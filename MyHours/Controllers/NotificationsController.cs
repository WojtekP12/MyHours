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
    public class NotificationsController : BaseController
    {

        // GET: Notifications
        public ActionResult Index()
        {
            var uSER_NOTIFICATION = db.USER_NOTIFICATION.Include(u => u.SUBJECT_ASSIGNMENT_TEMP).Include(u=>u.SUBJECT_ASSIGNMENT_TEMP.SUBJECT).Include(u=>u.USER);
            var userId = GetUserID();
            return View(uSER_NOTIFICATION.Where(x=>x.UserID == userId && x.StatusID == 1).ToList());
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            if (uSER_NOTIFICATION == null)
            {
                return RedirectToAction("Index");
            }
            return View(uSER_NOTIFICATION);
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            if (uSER_NOTIFICATION == null)
            {
                return RedirectToAction("Index");
            }
            return View(uSER_NOTIFICATION);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            if(uSER_NOTIFICATION == null)
            {
                return RedirectToAction("Index");
            }

            if(uSER_NOTIFICATION.Name == "added")
            {
                SUBJECT_ASSIGNMENT_TEMP subject = db.SUBJECT_ASSIGNMENT_TEMP.Find(uSER_NOTIFICATION.SubjectAssignmentTempID);
                db.SUBJECT_ASSIGNMENT_TEMP.Remove(subject);
            }

            uSER_NOTIFICATION.StatusID = 2;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Notifications/Accept/5
        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            if (uSER_NOTIFICATION == null)
            {
                return RedirectToAction("Index");
            }
            return View(uSER_NOTIFICATION);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptConfirmed(int id)
        {
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);

            if (uSER_NOTIFICATION == null)
            {
                return RedirectToAction("Index");
            }

            if (uSER_NOTIFICATION.Name.Contains("added"))
            {
                SUBJECT_ASSIGNMENT_TEMP subject = db.SUBJECT_ASSIGNMENT_TEMP.Find(uSER_NOTIFICATION.SubjectAssignmentTempID);
                SUBJECT_ASSIGNMENT resultSubject = new SUBJECT_ASSIGNMENT
                {
                    Hours = subject.Hours,
                    IsSubstitute = subject.IsSubstitute,
                    IsSubstituteDescription = subject.IsSubstituteDescription,
                    Semester = subject.Semester,
                    StudentGroupID = subject.StudentGroupID,
                    StudiesTypeID = subject.StudiesTypeID,
                    SubjectID = subject.SubjectID,
                    SubjectTypeID = subject.SubjectTypeID,
                    TeacherID = subject.TeacherID,
                    ReplacedName = subject.ReplacedName
                };

                var s = db.SUBJECT.Find(resultSubject.SubjectID);
                s.UsedHours += resultSubject.Hours;

                db.SUBJECT_ASSIGNMENT.Add(resultSubject);
                db.SUBJECT_ASSIGNMENT_TEMP.Remove(subject);
            }
            else if(uSER_NOTIFICATION.Name.Contains("deleted"))
            {
                var subject = db.SUBJECT_ASSIGNMENT.Find(uSER_NOTIFICATION.SubjectAssignmentID);
                var s = subject.SUBJECT;
                s.UsedHours -= subject.Hours;

                db.SUBJECT_ASSIGNMENT.Remove(subject);
            }
            else if(uSER_NOTIFICATION.Name.Contains("modified"))
            {
                SUBJECT_ASSIGNMENT_TEMP subject_temp = uSER_NOTIFICATION.SUBJECT_ASSIGNMENT_TEMP;
                SUBJECT_ASSIGNMENT subject = db.SUBJECT_ASSIGNMENT.Find(uSER_NOTIFICATION.SubjectAssignmentID);

                int hours = subject_temp.Hours - subject.Hours;
                var s = db.SUBJECT.Find(subject_temp.SubjectID);
                s.UsedHours += hours;

                subject.Hours = subject_temp.Hours;
                subject.IsSubstitute = subject_temp.IsSubstitute;
                subject.IsSubstituteDescription = subject_temp.IsSubstituteDescription;
                subject.Semester = subject_temp.Semester;
                subject.StudentGroupID = subject_temp.StudentGroupID;
                subject.StudiesTypeID = subject_temp.StudiesTypeID;
                subject.SubjectID = subject_temp.SubjectID;
                subject.SubjectTypeID = subject_temp.SubjectTypeID;
                subject.TeacherID = subject_temp.TeacherID;
                subject.ReplacedName = subject_temp.ReplacedName;

                db.SaveChanges();

                db.SUBJECT_ASSIGNMENT_TEMP.Remove(subject_temp);
            }

            uSER_NOTIFICATION.StatusID = 3;
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
