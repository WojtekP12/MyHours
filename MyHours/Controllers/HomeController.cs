using MyHours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MyHours.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private TAM_DBEntities db = new TAM_DBEntities();

        public ActionResult Index()
        {
            string id = getCurrentUserId();
            var userId = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault().ID;
            ViewBag.NotificationsCount = db.USER_NOTIFICATION.Where(x => x.UserID == userId && x.StatusID==1).Count();

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

        public JsonResult GetNotifications()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetNotifications(1);
            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        protected string getCurrentUserId()
        {
            string id = string.Empty;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                // the principal identity is a claims identity.
                // now we need to find the NameIdentifier claim
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    id = userIdClaim.Value;
                }
            }

            return id;
        }
    }
}