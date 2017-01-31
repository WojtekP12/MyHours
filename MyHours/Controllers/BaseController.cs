using MyHours.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace MyHours.Controllers
{
    public class BaseController : Controller
    {
        protected static bool DataBaseRequestOccured { get; set; }
        protected TAM_DBEntities db;
        public BaseController()
        {
            db = new TAM_DBEntities();
        }

        protected void GetUserNotificationCountFromDataBase()
        {
            if (User != null/* && DataBaseRequestOccured!=true*/)
            {
                var userId = GetUserID();
                var notificationCount = db.USER_NOTIFICATION.Where(x => x.UserID == userId && x.StatusID==1).Count();
                var teacherNotificationCount = db.USER_NOTIFICATION.Where(x => x.SenderID == userId).Count();
                Session["NotificationCount"] = notificationCount;
                Session["TeacherNotificationCount"] = teacherNotificationCount;
                //DataBaseRequestOccured = true;
            }
        }

        protected int GetUserID()
        {
            string id = GetCurrentUserId();
            var userId = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault().ID;
            return userId;
        }

        private string GetCurrentUserId()
        {
            string id = string.Empty;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    id = userIdClaim.Value;
                }
            }
            return id;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            GetUserNotificationCountFromDataBase();
        }

        [HttpPost]
        public ActionResult SetNotificationCount(int value)
        {
            Session["NotificationCount"] = value;
            Session["TeacherNotificationCount"] = value;
            return null;
        }
    }
}