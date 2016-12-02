using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyHours.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace MyHours.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private TAM_DBEntities db = new TAM_DBEntities();
        
        // GET: Teacher
        public ActionResult Index()
        {
            string id = getCurrentUserId();
            int totalTime;
            int usedTime;
            int freeTime;

            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).Include(s=>s.SUBJECT_TYPE_DICT).Include(s=>s.STUDIES_TYPE_DICT);
            var teacherId = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault().TeacherID;

            totalTime = db.TEACHER.Where(x => x.ID == teacherId).FirstOrDefault().AssignedHours;
            usedTime = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherId).Sum(x => x.Hours);
            freeTime = totalTime - usedTime;
            if(freeTime<0)
            {
                freeTime = 0;
            }

            ViewBag.TotalTime = totalTime;
            ViewBag.UsedTime = usedTime;
            ViewBag.FreeTime = freeTime;

            return View(sUBJECT_ASSIGNMENT.Where(x=>x.TeacherID== teacherId).ToList());
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
            ViewBag.SubjectTypeID = new SelectList(db.SUBJECT_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.SubjectTypeID);
            ViewBag.StudiesTypeID = new SelectList(db.STUDIES_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.StudiesTypeID);
            return View(sUBJECT_ASSIGNMENT);
        }

        // POST: Teacher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TeacherID,IsSubstitute,IsSubstituteDescription,StudentGroupID,SubjectID,SubjectTypeID, StudiesTypeID, Semester, Hours")] SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT)
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
            ViewBag.SubjectTypeID = new SelectList(db.SUBJECT_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.SubjectTypeID);
            ViewBag.StudiesTypeID = new SelectList(db.STUDIES_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.StudiesTypeID);
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
