using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCWAdminPortalWeb.Controllers.Web
{
    public class AppController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult ManageFeaturedProperties()
        {
            //ViewBag.Message = "Manage Featured Properties";

            return View();
        }*/

        public ActionResult ManageContactInfo()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ManageAgents()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}