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
        private static bool DataBaseRequestOccured { get; set; }
        protected TAM_DBEntities db;
        public BaseController()
        {
            db = new TAM_DBEntities();
            var u = User;
        }

        protected void GetUserNotificationCountFromDataBase()
        {
            if (User != null && DataBaseRequestOccured!=true)
            {
                string id = GetCurrentUserId();
                var userId = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault().ID;
                GlobalUserData.NotificationCount = db.USER_NOTIFICATION.Where(x => x.UserID == userId).Count();
                DataBaseRequestOccured = true;
            }
        }

        private string GetCurrentUserId()
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            GetUserNotificationCountFromDataBase();
            ViewBag.NotificationCount = GlobalUserData.NotificationCount;
        }

        [HttpPost]
        public ActionResult SetNotificationCount(int value)
        {
            GlobalUserData.NotificationCount = value;
            return null;
        }
    }
}