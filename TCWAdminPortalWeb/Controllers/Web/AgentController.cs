using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;

namespace TCWAdminPortalWeb.Controllers.Web
{
    public class AgentController : Controller
    {
        private TCWAdminRepository<Agent> _repository;

        public AgentController()
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

            //Instantiate a repository of type Agent
            _repository = new TCWAdminRepository<Agent>(imagesBlobContainer, imagesQueue);
        }

        // GET: agent
        public ActionResult Index()
        {
            var agents = _repository.GetAll();

            //use static instance of autoMapper to Map the View to the ViewModel
            var agentsVM = AutoMapperConfig.TCWMapper.Map<IEnumerable<Agent>, IEnumerable<AgentViewModel>>(agents);

            return View(agentsVM);
        }

        // GET: agent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: agent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AgentViewModel vm, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //insert the image into the blob
                    CloudBlockBlob imageBlob = await _repository.InsertImageBlob(imageFile);
                    if (imageBlob != null)
                    {
                        vm.ImageUrl = imageBlob.Uri.ToString();
                    }

                    //map viewmodel to view
                    var agent = AutoMapperConfig.TCWMapper.Map<Agent>(vm);

                    //now save model to db
                    _repository.Insert(agent);
                    await _repository.SaveAsync();

                    if (imageBlob != null)
                    {
                        //no need to call await here, because the AddMessageToQueue method already handles that
                        _repository.AddMessageToQueue(agent.ID.ToString(), typeof(Agent).ToString());
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

        // GET: agent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var agent = _repository.GetById(id);
            if (agent == null)
            {
                return HttpNotFound();
            }

            // found item so map it to the view model and return
            var vm = AutoMapperConfig.TCWMapper.Map<AgentViewModel>(agent);
            return View(vm);
        }

        // GET: agent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var agent = _repository.GetById(id);
            if (agent == null)
            {
                return HttpNotFound();
            }

            // found item so map it to the view model and return
            var vm = AutoMapperConfig.TCWMapper.Map<AgentViewModel>(agent);
            return View(vm);
        }

        // POST: agent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                var agent = _repository.GetById(id);

                //First delete the image from the blob
                await _repository.DeleteBlobsAsync(agent.ImageUrl, agent.ThumbnailURL);

                // then delete the featured property record itself
                _repository.Delete(id);
                _repository.Save();
            }
            catch (Exception ex)
            {
                //TODO Add logging and error handeling
            }
            return RedirectToAction("Index");
        }

        // GET: agent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var agent = _repository.GetById(id);
            if (agent == null)
            {
                return HttpNotFound();
            }

            // found item so map it to the view model and return
            var vm = AutoMapperConfig.TCWMapper.Map<AgentViewModel>(agent);
            return View(vm);
        }

        // POST: agent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AgentViewModel vm, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CloudBlockBlob imageBlob = null;
                    if (imageFile != null && imageFile.ContentLength != 0)
                    {
                        // User is changing the image -- delete the existing
                        // image blobs and then upload and save a new one.
                        await _repository.DeleteBlobsAsync(vm.ImageUrl, vm.ThumbnailURL);
                        imageBlob = await _repository.InsertImageBlob(imageFile);
                        if (imageBlob != null)
                        {
                            vm.ImageUrl = imageBlob.Uri.ToString();
                        }
                    }

                    //map viewmodel to view
                    var agent = AutoMapperConfig.TCWMapper.Map<Agent>(vm);

                    //now save model to db
                    _repository.Update(agent);
                    await _repository.SaveAsync();

                    if (imageBlob != null)
                    {
                        _repository.AddMessageToQueue(agent.ID.ToString(), typeof(Agent).ToString());
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
