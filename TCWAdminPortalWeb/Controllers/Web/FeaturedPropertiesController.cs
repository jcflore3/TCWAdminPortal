using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Web;
using System.Threading.Tasks;

namespace TCWAdminPortalWeb.Controllers.Web
{
    public class FeaturedPropertiesController : Controller
    {
        private ITCWAdminRepository<FeaturedProperty> _repository;

        public FeaturedPropertiesController()
        {
            //Initialize the Azure Storage first so the repository has what it needs to access the Blob Storage
            // Open storage account using credentials from .cscfg file.
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            // Get context object for working with blobs, and 
            // set a default retry policy appropriate for a web user interface.
            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the blob container.
            CloudBlobContainer imagesBlobContainer = blobClient.GetContainerReference("images");

            // Get context object for working with queues, and 
            // set a default retry policy appropriate for a web user interface.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the queue.
            CloudQueue imagesQueue = queueClient.GetQueueReference("images");

            //Instantiate a repository of type FeaturedProperty
            _repository = new TCWAdminRepository<FeaturedProperty>(imagesBlobContainer, imagesQueue);
            
        }

        // GET: FeaturedProperties
        public ActionResult Index()
        {
            var featuredProperties = _repository.GetAll();

            //use static instance of autoMapper to Map the View to the ViewModel
            var featuredPropertiesVM = AutoMapperConfig.TCWMapper.Map<IEnumerable<FeaturedProperty>, IEnumerable<FeaturedPropertyViewModel>>(featuredProperties);

            return View(featuredPropertiesVM);
        }

        // GET: FeaturedProperties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeaturedProperties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FeaturedPropertyViewModel vm, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CloudBlockBlob imageBlob = await _repository.InsertImageBlob(imageFile);
                    if (imageBlob != null)
                    {
                        vm.ImageURL = imageBlob.Uri.ToString();
                    }

                    //map viewmodel to view
                    var featuredProp = AutoMapperConfig.TCWMapper.Map<FeaturedProperty>(vm);

                    //now save model to db
                    _repository.Insert(featuredProp);
                    await _repository.SaveAsync();

                    if (imageBlob != null)
                    {
                        _repository.AddMessageToQueue(featuredProp.ID.ToString());
                    }

                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                //TODO: add logging
            }
            return View(vm);
        }

        // GET: FeaturedProperties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var featuredProperty = _repository.GetById(id);
            if (featuredProperty == null)
            {
                return HttpNotFound();
            }

            // found item so map it to the view model and return
            var vm = AutoMapperConfig.TCWMapper.Map<FeaturedPropertyViewModel>(featuredProperty);
            return View(vm);
        }

        // GET: FeaturedProperties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var featuredProperty = _repository.GetById(id);
            if (featuredProperty == null)
            {
                return HttpNotFound();
            }

            // found item so map it to the view model and return
            var vm = AutoMapperConfig.TCWMapper.Map<FeaturedPropertyViewModel>(featuredProperty);
            return View(vm);
        }

        // POST: FeaturedProperties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            try {
                var featuredProp = _repository.GetById(id);

                //First delete the image from the blob
                _repository.DeleteBlobsAsync(featuredProp.ImageURL, featuredProp.ThumbnailURL);

                // then delete the featured property record itself
                _repository.Delete(id);
                _repository.Save();
            }
            catch(Exception ex)
            {
                //TODO Add logging and error handeling
            }
            return RedirectToAction("Index");
        }

        // GET: FeaturedProperties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var featuredProperty = _repository.GetById(id);
            if (featuredProperty == null)
            {
                return HttpNotFound();
            }

            // found item so map it to the view model and return
            var vm = AutoMapperConfig.TCWMapper.Map<FeaturedPropertyViewModel>(featuredProperty);
            return View(vm);
        }

        // POST: FeaturedProperties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FeaturedPropertyViewModel vm, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CloudBlockBlob imageBlob = await _repository.InsertImageBlob(imageFile);
                    if (imageBlob != null)
                    {
                        vm.ImageURL = imageBlob.Uri.ToString();
                    }

                    //map viewmodel to view
                    var featuredProp = AutoMapperConfig.TCWMapper.Map<FeaturedProperty>(vm);

                    //now save model to db
                    _repository.Update(featuredProp);
                    await _repository.SaveAsync();

                    if (imageBlob != null)
                    {
                        _repository.AddMessageToQueue(featuredProp.ID.ToString());
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }
            return View(vm);
        }
    }
}