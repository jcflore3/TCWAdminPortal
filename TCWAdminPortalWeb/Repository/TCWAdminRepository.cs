using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage.Blob;
using TCWAdminPortalWeb.Models;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Diagnostics;
using System.IO;

namespace TCWAdminPortalWeb.Repository
{
    /// <summary>
    /// Generic Repository, that implements the ITCWAdminRepository, to be used with all 
    /// TCWAdmin objects to get and modify data in the database.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TCWAdminRepository<T> : ITCWAdminRepository<T> where T : class
    {

        private TCWAdminContext _dbContext;
        private DbSet<T> _dbSet;
        private static CloudBlobContainer _imagesBlobContainer;
        private CloudQueue _imagesQueue;

        public TCWAdminRepository()
        {
            _dbContext = new TCWAdminContext();
            _dbSet = _dbContext.Set<T>();
        }

        public TCWAdminRepository(CloudBlobContainer imagesBlobContainer, CloudQueue imagesQueue)
        {
            _dbContext = new TCWAdminContext();
            _dbSet = _dbContext.Set<T>();
            _imagesBlobContainer = imagesBlobContainer;
            _imagesQueue = imagesQueue;
        }

        public void Delete(object Id)
        {
            T getObjById = _dbSet.Find(Id);
            _dbSet.Remove(getObjById);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetFirst()
        {
            return _dbSet.First();
        }

        public T GetById(object Id)
        {
            return _dbSet.Find(Id);
        }

        public void Insert(T obj)
        {
            _dbSet.Add(obj);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            _dbContext.Entry(obj).State = EntityState.Modified;
        }

        public async Task<CloudBlockBlob> InsertImageBlob(HttpPostedFileBase imageFile)
        {
            CloudBlockBlob imageBlob = null;

            //TODO: add more input validation like checking that the image file is not too large
            if (imageFile != null && imageFile.ContentLength != 0)
            {
                imageBlob = await UploadAndSaveBlobAsync(imageFile);
            }

            return imageBlob;
        }

        /// <summary>
        /// Adds a message to the worker role queue
        /// </summary>
        /// <param name="IdString">The Id of the record the queue message corresponds to</param>
        /// <param name="objTypeStr">The string type of the record id passed in</param>
        /// <returns></returns>
        public async Task AddMessageToQueue(string IdString, string objTypeStr)
        {
            //set the message to only the ID of the record the image belongs too.
            //but, we also need a way to differentiate the type of record so the 
            //worker process knows which table to look in. Therefore add the type
            //to the end of the id.
            var queueMessage = new CloudQueueMessage(IdString + '-' + objTypeStr);
            await _imagesQueue.AddMessageAsync(queueMessage);
            Trace.TraceInformation("Created queue message for AdId {0}", IdString);
        }

        public async Task<CloudBlockBlob> UploadAndSaveBlobAsync(HttpPostedFileBase imageFile)
        {
            Trace.TraceInformation("Uploading image file {0}", imageFile.FileName);

            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            // Retrieve reference to a blob. 
            CloudBlockBlob imageBlob = _imagesBlobContainer.GetBlockBlobReference(blobName);
            // Create the blob by uploading a local file.
            using (var fileStream = imageFile.InputStream)
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
            }

            Trace.TraceInformation("Uploaded image file to {0}", imageBlob.Uri.ToString());

            return imageBlob;
        }

        public async Task DeleteBlobsAsync(string imageURL, string thumbnailURL)
        {
            if (!string.IsNullOrWhiteSpace(imageURL))
            {
                Uri blobUri = new Uri(imageURL);
                await DeleteBlobAsync(blobUri);
            }
            if (!string.IsNullOrWhiteSpace(thumbnailURL))
            {
                Uri blobUri = new Uri(thumbnailURL);
                await DeleteBlobAsync(blobUri);
            }
        }

        public async Task DeleteBlobAsync(Uri blobUri)
        {
            try {
                string blobName = blobUri.Segments[blobUri.Segments.Length - 1];
                Trace.TraceInformation("Deleting image blob {0}", blobName);
                CloudBlockBlob blobToDelete = _imagesBlobContainer.GetBlockBlobReference(blobName);
                await blobToDelete.DeleteAsync();
            }
            catch(Exception ex)
            {
                //TODO: put in some logging
                Debug.WriteLine("Failed to Delete image and thumbail" + ex);
                Trace.TraceInformation("Unable to delete, probably because the image no longer exists in the blob");
            }
        }
    }
}
