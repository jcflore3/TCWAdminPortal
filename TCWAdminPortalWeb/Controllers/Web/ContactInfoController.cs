using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCWAdminPortalWeb.Models;
using TCWAdminPortalWeb.Repository;
using TCWAdminPortalWeb.ViewModels;

namespace TCWAdminPortalWeb.Controllers.Web
{
    [Authorize]
    public class ContactInfoController : Controller
    {
        private TCWAdminRepository<ContactInfo> _repository;

        public ContactInfoController()
        {
            _repository = new TCWAdminRepository<ContactInfo>();
        }

        /// <summary>
        /// Get: ContactInfo
        /// Details action for the Details View. No need to pass in an ID because
        /// there should only be one record in the DB. So this just returns the first.
        /// </summary>
        /// <returns>Only the first Contact Info record in the table (there should only be on in the table anyway)</returns>
        public ActionResult Details()
        {
            return View(GetContactInfoAndMap());
        }

        /// <summary>
        /// Get: ContactInfo
        /// Edit action for the Edit View. No need to pass in an ID because
        /// there should only be one record in the DB. So this just returns the first.
        /// </summary>
        /// <returns>Only the first Contact Info record in the table (there should only be on in the table anyway)</returns>
        public ActionResult Edit()
        {
            return View(GetContactInfoAndMap());
        }

        // POST: ContactInfo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactInfoViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //map viewmodel to view
                    var contactInfo = AutoMapperConfig.TCWMapper.Map<ContactInfo>(vm);

                    //now save model to db
                    _repository.Update(contactInfo);
                    _repository.Save();

                    return RedirectToAction("Details");
                }
            }
            catch (Exception ex)
            {
                //TODO: add logging
            }
            return View(vm);
        }

        /// <summary>
        /// Helper method that gets the first Contact Info Record and
        /// Maps it to the contact info viewModel
        /// </summary>
        /// <returns>ContactInfoViewModel</returns>
        private ContactInfoViewModel GetContactInfoAndMap()
        {
            // get the first contact info record using the generic repository
            // (there should only be one contact info record at any time)
            var contactInfo = _repository.GetFirst();

            // now that we have the record, we need to map it to the viewModel
            var contactInfoVM = AutoMapperConfig.TCWMapper.Map<ContactInfoViewModel>(contactInfo);

            // return the View with the contact info ViewModel
            return contactInfoVM;
        }
    }
}