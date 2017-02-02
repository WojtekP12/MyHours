using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyHours.Models;

namespace MyHours.Controllers
{
    public class TeacherNotificationController : BaseController
    {
        
        // GET: TeacherNotification
        public ActionResult Index()
        {
            var userId = GetUserID();
            var uSER_NOTIFICATION = db.USER_NOTIFICATION.Include(u => u.SUBJECT_ASSIGNMENT_TEMP).Include(u => u.SUBJECT_ASSIGNMENT_TEMP.SUBJECT).Include(u => u.USER).Include(u=>u.NOTIFICATION_STATUS);

            return View(uSER_NOTIFICATION.Where(x=>x.SenderID == userId).ToList());
        }

        // GET: TeacherNotification/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            if (uSER_NOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(uSER_NOTIFICATION);
        }

        // GET: TeacherNotification/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            if (uSER_NOTIFICATION == null)
            {
                return HttpNotFound();
            }
            return View(uSER_NOTIFICATION);
        }

        // POST: TeacherNotification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);

            var subject = db.SUBJECT_ASSIGNMENT_TEMP.Find(uSER_NOTIFICATION.SubjectAssignmentTempID);
            if(subject!=null)
            {
                db.SUBJECT_ASSIGNMENT_TEMP.Remove(subject);
            }

            db.USER_NOTIFICATION.Remove(uSER_NOTIFICATION);


            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
