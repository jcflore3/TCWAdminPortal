using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using TCWAdminPortalWeb.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TCWAdminPortalWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        //private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private CloudQueue _imagesQueue;
        private CloudBlobContainer _imagesBlobContainer;
        private TCWAdminContext _dbContext;

        public override void Run()
        {
            Trace.TraceInformation("TCWAdminPortalWorker is running");
            CloudQueueMessage msg = null;

            // To make the worker role more scalable, implement multi-threaded and 
            // asynchronous code. See:
            // http://msdn.microsoft.com/en-us/library/ck8bc5c6.aspx
            // http://www.asp.net/aspnet/overview/developing-apps-with-windows-azure/building-real-world-cloud-apps-with-windows-azure/web-development-best-practices#async

            while (true)
            {
                try {
                    //Retrieve a new message from the queue
                    //If this App gets to be quite large, then use getMessages instead
                    // so that multiple queued messages are pulled at once. Don't forsee this happeining though
                    msg = _imagesQueue.GetMessage();
                    if (msg != null)
                    {
                        ProcessQueueMessage(msg);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (StorageException e)
                {
                    if (msg != null && msg.DequeueCount > 5)
                    {
                        this._imagesQueue.DeleteMessage(msg);
                        Trace.TraceError("Deleting poison queue item: '{0}'", msg.AsString);
                    }
                    Trace.TraceError("Exception in TCWAdminWorker: '{0}'", e.Message);
                    System.Threading.Thread.Sleep(5000);
                }
            }
            /*try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }*/
        }

        private void ProcessQueueMessage(CloudQueueMessage msg)
        {
            Trace.TraceInformation("Processing queue message {0}", msg);

            // process the message
            var msgString = msg.AsString;
            string[] msgStrArray = msgString.Split('-');

            var recordId = int.Parse(msgStrArray[0]);
            Uri blobUri = null;
            FeaturedProperty featuredProp = null;
            Agent agentRecord = null;
            if (msgStrArray != null && msgStrArray.Length > 1)
            {
                //TODO: this is ugly, have to find a better way to identify and process the message type
                if (typeof(FeaturedProperty).ToString().Equals(msgStrArray[1]))
                {
                    featuredProp = _dbContext.FeaturedProperties.Find(recordId);
                    // Reload the context just to be safe
                    _dbContext.Entry(featuredProp).Reload();
                    if (featuredProp == null)
                    {
                        throw new Exception(String.Format("Feature Property Id {0} not found, can't create thumbnail", recordId.ToString()));
                    }
                    blobUri = new Uri(featuredProp.ImageURL);
                }
                else if (typeof(Agent).ToString().Equals(msgStrArray[1]))
                {
                    agentRecord = _dbContext.Agents.Find(recordId);
                    // Reload the context just to be safe
                    _dbContext.Entry(agentRecord).Reload();
                    if (agentRecord == null)
                    {
                        throw new Exception(String.Format("Agent Id {0} not found, can't create thumbnail", recordId.ToString()));
                    }
                    blobUri = new Uri(agentRecord.ImageUrl);
                }
                else
                {
                    //type not handled yet so just return
                    return;
                }
            }
            else
            {
                //the queue message did not include both a record id and a string type,
                //therefore we don't know which type of message we are processing so return 
                //and do not process anything.
                return;
            }

            string blobName = blobUri.Segments[blobUri.Segments.Length - 1];

            CloudBlockBlob inputBlob = _imagesBlobContainer.GetBlockBlobReference(blobName);
            string thumbnailName = Path.GetFileNameWithoutExtension(inputBlob.Name) + "thumb.jpg";
            CloudBlockBlob outputBlob = _imagesBlobContainer.GetBlockBlobReference(thumbnailName);

            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
                ConvertImageToThumbnailJPG(input, output);
                outputBlob.Properties.ContentType = "image/jpeg";
            }
            Trace.TraceInformation("Generated thumbnail in blob {0}", thumbnailName);

            //TODO: Again, this is ugly but it works. Need to find a better way
            if (featuredProp != null)
            {
                featuredProp.ThumbnailURL = outputBlob.Uri.ToString();
            }
            else if (agentRecord != null)
            {
                agentRecord.ThumbnailURL = outputBlob.Uri.ToString();
            }
            _dbContext.SaveChanges();
            Trace.TraceInformation("Updated thumbnail URL in database: {0}", outputBlob.Uri.ToString());

            // Remove message from queue.
            _imagesQueue.DeleteMessage(msg);
        }

        private void ConvertImageToThumbnailJPG(Stream input, Stream output)
        {
            int thumbnailsize = 80;
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = thumbnailsize;
                height = thumbnailsize * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = thumbnailsize;
                width = thumbnailsize * originalImage.Width / originalImage.Height;
            }

            Bitmap thumbnailImage = null;
            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                thumbnailImage.Save(output, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections.
            ServicePointManager.DefaultConnectionLimit = 12;

            // Read database connection string and open database.
            var dbConnString = CloudConfigurationManager.GetSetting("TCWAdminPortalDbConnectionString");
            _dbContext = new TCWAdminContext(dbConnString);

            // Open storage account using credentials from .cscfg file.
            var storageAccount = CloudStorageAccount.Parse
                (RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            Trace.TraceInformation("Creating images blob container");
            var blobClient = storageAccount.CreateCloudBlobClient();
            _imagesBlobContainer = blobClient.GetContainerReference("images");
            if (_imagesBlobContainer.CreateIfNotExists())
            {
                // Enable public access on the newly created "images" container.
                _imagesBlobContainer.SetPermissions(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
            }

            Trace.TraceInformation("Creating images queue");
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            _imagesQueue = queueClient.GetQueueReference("images");
            _imagesQueue.CreateIfNotExists();

            Trace.TraceInformation("Storage initialized");
            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("TCWAdminPortalWorker is stopping");

            //this.cancellationTokenSource.Cancel();
            //this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("TCWAdminPortalWorker has stopped");
        }

        /*private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }*/
    }
}
