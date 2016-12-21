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
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private TAM_DBEntities db = new TAM_DBEntities();

        // GET: Administrator
        public ActionResult Index()
        {
            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).GroupBy(s=>s.TeacherID).Select(s => s.FirstOrDefault());

            return View(sUBJECT_ASSIGNMENT.ToList());
        }

        // GET: Administrator/Details/5
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

        // GET: Administrator/Create
        public ActionResult Create()
        {
            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name");
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "SubjectCode");
            ViewBag.TeacherID = new SelectList(db.TEACHER, "ID", "FirstName");
            return View();
        }

        // POST: Administrator/Create
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

        // GET: Administrator/Edit/5
        public ActionResult Edit(int? id)
        {
            int totalTime;
            int usedTime;
            int freeTime;

            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).Include(s => s.SUBJECT_TYPE_DICT).Include(s => s.STUDIES_TYPE_DICT);

            totalTime = db.TEACHER.Where(x => x.ID == id).FirstOrDefault().AssignedHours;
            usedTime = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == id).Sum(x => x.Hours);
            freeTime = totalTime - usedTime;

            if (freeTime < 0)
            {
                freeTime = 0;
            }

            ViewBag.TotalTime = totalTime;
            ViewBag.UsedTime = usedTime;
            ViewBag.FreeTime = freeTime;
            ViewBag.Teacher = db.TEACHER.Where(x => x.ID == id).FirstOrDefault().TeacherStatus + " " + db.TEACHER.Where(x => x.ID == id).FirstOrDefault().FirstName + " " + db.TEACHER.Where(x => x.ID == id).FirstOrDefault().SecondName;

            return View(sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == id).ToList());
        }

        // GET: Administrator/Delete/5
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

        // POST: Administrator/Delete/5
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
