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
    [Authorize(Roles = "Teacher, Administrator")]
    public class TeacherController : BaseController
    {
        // GET: Teacher
        public ActionResult Index()
        {
            string id = getCurrentUserId();
            int totalTime;
            int overTime;
            int numberOfSubjects;

            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).Include(s=>s.SUBJECT_TYPE_DICT).Include(s=>s.STUDIES_TYPE_DICT);
            var teacherId = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault().TeacherID;

            totalTime = db.TEACHER.Where(x => x.ID == teacherId).FirstOrDefault().AssignedHours;
            overTime = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherId).Sum(x => x.Hours) - totalTime;
            overTime = overTime < 0 ? 0 : overTime;

            numberOfSubjects = db.SUBJECT_ASSIGNMENT.Where(x=>x.TeacherID == teacherId).Count();

            ViewBag.TotalTime = totalTime;
            ViewBag.OverTime = overTime;
            ViewBag.NumberOfSubjects = numberOfSubjects;

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
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "Name");
            ViewBag.TeacherID = new SelectList(db.TEACHER, "ID", "FirstName");
            ViewBag.SubjectTypeID = new SelectList(db.SUBJECT_TYPE_DICT, "ID", "Description");
            ViewBag.StudiesTypeID = new SelectList(db.STUDIES_TYPE_DICT, "ID", "Description");

            return View(new SUBJECT_ASSIGNMENT());
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TeacherID,IsSubstitute,IsSubstituteDescription,StudentGroupID,SubjectID,SubjectTypeID, StudiesTypeID, Semester, Hours")] SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT)
        {
            if (ModelState.IsValid)
            {

                SUBJECT_ASSIGNMENT_TEMP subjectAssignmentTemp = new SUBJECT_ASSIGNMENT_TEMP();

                string id = getCurrentUserId();
                var user = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault();
                var teacherId = user.TeacherID;

                subjectAssignmentTemp.TeacherID = teacherId.Value;

                //sUBJECT_ASSIGNMENT.TeacherID = teacherId.Value;
                subjectAssignmentTemp.Hours = sUBJECT_ASSIGNMENT.Hours;
                subjectAssignmentTemp.IsSubstitute = sUBJECT_ASSIGNMENT.IsSubstitute;
                subjectAssignmentTemp.IsSubstituteDescription = sUBJECT_ASSIGNMENT.IsSubstituteDescription;
                subjectAssignmentTemp.ProxyID = sUBJECT_ASSIGNMENT.ProxyID;
                subjectAssignmentTemp.Semester = sUBJECT_ASSIGNMENT.Semester;
                subjectAssignmentTemp.StudentGroupID = sUBJECT_ASSIGNMENT.StudentGroupID;
                subjectAssignmentTemp.StudiesTypeID = sUBJECT_ASSIGNMENT.StudiesTypeID;
                subjectAssignmentTemp.SubjectID = sUBJECT_ASSIGNMENT.SubjectID;
                subjectAssignmentTemp.SubjectTypeID = sUBJECT_ASSIGNMENT.SubjectTypeID;

                db.SUBJECT_ASSIGNMENT_TEMP.Add(subjectAssignmentTemp);
                //db.SUBJECT_ASSIGNMENT.Add(sUBJECT_ASSIGNMENT);
                db.SaveChanges();

                var adminID = db.USER.Where(x => x.UserTypeID == 2).FirstOrDefault().ID;

                USER_NOTIFICATION noti = new USER_NOTIFICATION();
                noti.Date = DateTime.Now;
                noti.Name = "added";
                noti.Description = db.SUBJECT.Where(x=>x.ID == subjectAssignmentTemp.SubjectID).FirstOrDefault().Name + " added by " + user.Name;
                noti.SenderID = user.ID;
                noti.UserID = adminID;
                noti.StatusID = 1;
                noti.SubjectAssignmentTempID = subjectAssignmentTemp.ID;

                db.USER_NOTIFICATION.Add(noti);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name", sUBJECT_ASSIGNMENT.StudentGroupID);
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "Name", sUBJECT_ASSIGNMENT.SubjectID);
            ViewBag.SubjectTypeID = new SelectList(db.SUBJECT_TYPE_DICT, "ID", "Description",sUBJECT_ASSIGNMENT.SubjectTypeID);
            ViewBag.StudiesTypeID = new SelectList(db.STUDIES_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.StudiesTypeID);

            return View(sUBJECT_ASSIGNMENT);
        }

        // GET: Teacher/Edit/5
        public ActionResult Edit(int? id)
        {
            int assignedHours;
            int usedHours;
            int freeHours;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);

            var subject = db.SUBJECT.Find(sUBJECT_ASSIGNMENT.SubjectID);

            assignedHours = subject.AssignedHours;
            usedHours = subject.UsedHours;
            freeHours = (assignedHours - usedHours) < 0 ? 0 : (assignedHours - usedHours);

            ViewBag.AssignedHours = assignedHours;
            ViewBag.UsedHours = usedHours;
            ViewBag.FreeHours = freeHours;

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
                SUBJECT_ASSIGNMENT_TEMP subjectAssignmentTemp = new SUBJECT_ASSIGNMENT_TEMP();

                string id = getCurrentUserId();
                var user = db.USER.Where(x => x.AspNetUserID == id).FirstOrDefault();
                var teacherId = user.TeacherID;

                subjectAssignmentTemp.TeacherID = teacherId.Value;

                subjectAssignmentTemp.Hours = sUBJECT_ASSIGNMENT.Hours;
                subjectAssignmentTemp.IsSubstitute = sUBJECT_ASSIGNMENT.IsSubstitute;
                subjectAssignmentTemp.IsSubstituteDescription = sUBJECT_ASSIGNMENT.IsSubstituteDescription;
                subjectAssignmentTemp.ProxyID = sUBJECT_ASSIGNMENT.ProxyID;
                subjectAssignmentTemp.Semester = sUBJECT_ASSIGNMENT.Semester;
                subjectAssignmentTemp.StudentGroupID = sUBJECT_ASSIGNMENT.StudentGroupID;
                subjectAssignmentTemp.StudiesTypeID = sUBJECT_ASSIGNMENT.StudiesTypeID;
                subjectAssignmentTemp.SubjectID = sUBJECT_ASSIGNMENT.SubjectID;
                subjectAssignmentTemp.SubjectTypeID = sUBJECT_ASSIGNMENT.SubjectTypeID;

                db.SUBJECT_ASSIGNMENT_TEMP.Add(subjectAssignmentTemp);

                db.SaveChanges();

                var adminID = db.USER.Where(x => x.UserTypeID == 2).FirstOrDefault().ID;

                USER_NOTIFICATION noti = new USER_NOTIFICATION();
                noti.Date = DateTime.Now;
                noti.Name = "modified";
                noti.Description = db.SUBJECT.Where(x => x.ID == subjectAssignmentTemp.SubjectID).FirstOrDefault().Name + " modified by " + user.Name;
                noti.SenderID = user.ID;
                noti.UserID = adminID;
                noti.StatusID = 1;
                noti.SubjectAssignmentTempID = subjectAssignmentTemp.ID;
                noti.SubjectAssignmentID = sUBJECT_ASSIGNMENT.ID;

                db.USER_NOTIFICATION.Add(noti);
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
            string userId = getCurrentUserId();
            var user = db.USER.Where(x => x.AspNetUserID == userId).FirstOrDefault();
            var teacherId = user.TeacherID;
            var adminID = db.USER.Where(x => x.UserTypeID == 2).FirstOrDefault().ID;

            
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);

            USER_NOTIFICATION temp = db.USER_NOTIFICATION.Where(x => x.SubjectAssignmentID == sUBJECT_ASSIGNMENT.ID && x.Name.Contains("deleted")).FirstOrDefault();

            if(temp==null)
            {
                USER_NOTIFICATION noti = new USER_NOTIFICATION();
                noti.Date = DateTime.Now;
                noti.Name = "deleted";
                noti.Description = db.SUBJECT.Where(x => x.ID == sUBJECT_ASSIGNMENT.SubjectID).FirstOrDefault().Name + " deleted by " + user.Name;
                noti.SenderID = user.ID;
                noti.UserID = adminID;
                noti.StatusID = 1;
                noti.SubjectAssignmentID = sUBJECT_ASSIGNMENT.ID;

                db.USER_NOTIFICATION.Add(noti);
                db.SaveChanges();
            }
            
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
