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
        private TAM_DBEntities db = new TAM_DBEntities();

        // GET: Notifications
        public ActionResult Index()
        {
            var uSER_NOTIFICATION = db.USER_NOTIFICATION.Include(u => u.SUBJECT_ASSIGNMENT_TEMP).Include(u=>u.SUBJECT_ASSIGNMENT_TEMP.SUBJECT).Include(u=>u.USER);
            var userId = GetUserID();
            return View(uSER_NOTIFICATION.Where(x=>x.UserID == userId).ToList());
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
                return HttpNotFound();
            }
            return View(uSER_NOTIFICATION);
        }

        // GET: Notifications/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.SubjectAssignmentID = new SelectList(db.SUBJECT_ASSIGNMENT, "ID", "IsSubstituteDescription", uSER_NOTIFICATION.SubjectAssignmentID);
            return View(uSER_NOTIFICATION);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SenderID,UserID,Name,Description,StatusID,Date,SubjectAssignmentID")] USER_NOTIFICATION uSER_NOTIFICATION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSER_NOTIFICATION).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectAssignmentID = new SelectList(db.SUBJECT_ASSIGNMENT, "ID", "IsSubstituteDescription", uSER_NOTIFICATION.SubjectAssignmentID);
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
                return HttpNotFound();
            }
            return View(uSER_NOTIFICATION);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            SUBJECT_ASSIGNMENT_TEMP subject = db.SUBJECT_ASSIGNMENT_TEMP.Find(uSER_NOTIFICATION.SubjectAssignmentID);

            db.SUBJECT_ASSIGNMENT_TEMP.Remove(subject);
            db.USER_NOTIFICATION.Remove(uSER_NOTIFICATION);
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
                return HttpNotFound();
            }
            return View(uSER_NOTIFICATION);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Accept")]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptConfirmed(int id)
        {
            USER_NOTIFICATION uSER_NOTIFICATION = db.USER_NOTIFICATION.Find(id);
            SUBJECT_ASSIGNMENT_TEMP subject = db.SUBJECT_ASSIGNMENT_TEMP.Find(uSER_NOTIFICATION.SubjectAssignmentID);
            SUBJECT_ASSIGNMENT resultSubject = new SUBJECT_ASSIGNMENT
            {
                Hours = subject.Hours,
                IsSubstitute = subject.IsSubstitute,
                IsSubstituteDescription = subject.IsSubstituteDescription,
                ProxyID = subject.ProxyID,
                Semester = subject.Semester,
                StudentGroupID = subject.StudentGroupID,
                StudiesTypeID = subject.StudiesTypeID,
                SubjectID = subject.SubjectID,
                SubjectTypeID = subject.SubjectTypeID,
                TeacherID = subject.TeacherID
            };

            db.SUBJECT_ASSIGNMENT.Add(resultSubject);
            db.SUBJECT_ASSIGNMENT_TEMP.Remove(subject);
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
