using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            _repository = new TCWAdminRepository<Agent>();
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
        public ActionResult Create(AgentViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //map viewmodel to view
                    var agent = AutoMapperConfig.TCWMapper.Map<Agent>(vm);

                    //now save model to db
                    _repository.Insert(agent);
                    _repository.Save();

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
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                // have ID of property so delete it now
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
        public ActionResult Edit(AgentViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //map viewmodel to view
                    var agent = AutoMapperConfig.TCWMapper.Map<Agent>(vm);

                    //now save model to db
                    _repository.Update(agent);
                    _repository.Save();

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
