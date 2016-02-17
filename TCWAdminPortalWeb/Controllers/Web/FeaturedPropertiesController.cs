using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;

namespace TCWAdminPortalWeb.Controllers.Web
{
    public class FeaturedPropertiesController : Controller
    {
        private ITCWAdminRepository<FeaturedProperty> _repository;

        public FeaturedPropertiesController()
        {
            _repository = new TCWAdminRepository<FeaturedProperty>();
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
        public ActionResult Create(FeaturedPropertyViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //map viewmodel to view
                    var featuredProp = AutoMapperConfig.TCWMapper.Map<FeaturedProperty>(vm);

                    //now save model to db
                    _repository.Insert(featuredProp);
                    _repository.Save();

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
                // have ID of property so delete it now
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
        public ActionResult Edit(FeaturedPropertyViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //map viewmodel to view
                    var featuredProp = AutoMapperConfig.TCWMapper.Map<FeaturedProperty>(vm);

                    //now save model to db
                    _repository.Update(featuredProp);
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