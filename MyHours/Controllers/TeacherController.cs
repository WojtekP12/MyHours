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
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private TAM_DBEntities db = new TAM_DBEntities();
        
        // GET: Teacher
        public ActionResult Index()
        {
            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).Include(s=>s.SUBJECT_TYPE_DICT);
            return View(sUBJECT_ASSIGNMENT.ToList());
        }

        // GET: Teacher/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);
            if (sUBJECT_ASSIGNMENT == null)
            {
                return HttpNotFound();
            }
            return View(sUBJECT_ASSIGNMENT);
        }

        // GET: Teacher/Create
        public ActionResult Create()
        {
            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name");
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "SubjectCode");
            ViewBag.TeacherID = new SelectList(db.TEACHER, "ID", "FirstName");
            return View();
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Hours,TeacherID,IsSubstitute,IsSubstituteDescription,StudentGroupID,SubjectID")] SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT)
        {
            if (ModelState.IsValid)
            {
                db.SUBJECT_ASSIGNMENT.Add(sUBJECT_ASSIGNMENT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name", sUBJECT_ASSIGNMENT.StudentGroupID);
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "SubjectCode", sUBJECT_ASSIGNMENT.SubjectID);
            ViewBag.TeacherID = new SelectList(db.TEACHER, "ID", "FirstName", sUBJECT_ASSIGNMENT.TeacherID);
            return View(sUBJECT_ASSIGNMENT);
        }

        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);
            if (sUBJECT_ASSIGNMENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name", sUBJECT_ASSIGNMENT.StudentGroupID);
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "SubjectCode", sUBJECT_ASSIGNMENT.SubjectID);
            ViewBag.TeacherID = new SelectList(db.TEACHER, "ID", "FirstName", sUBJECT_ASSIGNMENT.TeacherID);
            return View(sUBJECT_ASSIGNMENT);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Hours,TeacherID,IsSubstitute,IsSubstituteDescription,StudentGroupID,SubjectID")] SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sUBJECT_ASSIGNMENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name", sUBJECT_ASSIGNMENT.StudentGroupID);
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "SubjectCode", sUBJECT_ASSIGNMENT.SubjectID);
            ViewBag.TeacherID = new SelectList(db.TEACHER, "ID", "FirstName", sUBJECT_ASSIGNMENT.TeacherID);
            return View(sUBJECT_ASSIGNMENT);
        }

        // GET: Teacher/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);
            if (sUBJECT_ASSIGNMENT == null)
            {
                return HttpNotFound();
            }
            return View(sUBJECT_ASSIGNMENT);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);
            db.SUBJECT_ASSIGNMENT.Remove(sUBJECT_ASSIGNMENT);
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
