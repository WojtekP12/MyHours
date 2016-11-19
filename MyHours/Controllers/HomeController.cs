using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyHours.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeacherPanel()
        {
            ViewBag.Message = "Your Teacher panel.";

            return View();
        }

        public ActionResult AdministratorPanel()
        {
            ViewBag.Message = "Your Administrator panel.";

            return View();
        }
    }
}