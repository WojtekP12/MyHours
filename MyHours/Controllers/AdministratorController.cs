using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyHours.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace MyHours.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : BaseController
    {
        int tempId;

        // GET: Administrator
        public ActionResult Index()
        {
            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).GroupBy(s=>s.TeacherID).Select(s => s.FirstOrDefault());

            var user = db.USER.Include(s => s.TEACHER);

            var u = User.IsInRole("Administrator");
            var u2 = User.IsInRole("Teacher");

            return View(user.Where(x=>x.UserTypeID==1).ToList());
        }


        // GET: Administrator/Edit/5
        public ActionResult Edit(int? id)
        {
            int totalTime;
            int overTime;
            int numberOfSubjects;

            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).Include(s => s.SUBJECT_TYPE_DICT).Include(s => s.STUDIES_TYPE_DICT);

            var teacherID = db.USER.Where(x => x.ID == id).FirstOrDefault().TeacherID;

            totalTime = db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().AssignedHours;
            var subjectAssignment = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).FirstOrDefault();

            overTime = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).Sum(x => x.Hours) - totalTime;
            overTime = overTime < 0 ? 0 : overTime;

            numberOfSubjects = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).Count();

            if (numberOfSubjects < 0)
            {
                numberOfSubjects = 0;
            }

            ViewBag.TotalTime = totalTime;
            ViewBag.OverTime = overTime;
            ViewBag.NumberOfSubjects = numberOfSubjects;
            ViewBag.Teacher = db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().TeacherStatus + " " + db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().FirstName + " " + db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().SecondName;

            return View(sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).ToList());
        }

        public ActionResult EditSubject(int? id)
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
        public ActionResult EditSubject([Bind(Include = "ID,TeacherID,IsSubstitute,IsSubstituteDescription,StudentGroupID,SubjectID,SubjectTypeID, StudiesTypeID, Semester, Hours")] SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT)
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
            USER user = db.USER.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Teacher = user.TEACHER.TeacherStatus + " " + user.FirstName + " " + user.SecondName;
            return View(user);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var teacherId = db.USER.Where(x => x.ID == id).FirstOrDefault().TeacherID;
            var aspUserId = db.USER.Where(x => x.ID == id).FirstOrDefault().AspNetUserID;
            var userType = db.USER_TYPE.Where(x => x.ID == db.USER.Where(y => y.ID == id).FirstOrDefault().UserTypeID).FirstOrDefault().Description;

            var assignedSubjects = db.SUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherId).ToList();


            if (assignedSubjects.Count != 0)
            {
                foreach (var subject in assignedSubjects)
                {
                    db.SUBJECT_ASSIGNMENT.Attach(subject);
                    db.SUBJECT_ASSIGNMENT.Remove(subject);
                }
                db.SaveChanges();
            }

            var teacher = db.TEACHER.Where(x => x.ID == teacherId).FirstOrDefault();
            db.TEACHER.Attach(teacher);
            db.TEACHER.Remove(teacher);
            db.SaveChanges();

            var user = db.USER.Where(x => x.ID == id).FirstOrDefault();
            db.USER.Attach(user);
            db.USER.Remove(user);
            db.SaveChanges();


            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.RemoveFromRole(aspUserId, userType);

                var aspUser = db.AspNetUsers.Where(x => x.Id == aspUserId).FirstOrDefault();
                db.AspNetUsers.Attach(aspUser);
                db.AspNetUsers.Remove(aspUser);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Teacher/Delete/5
        public ActionResult DeleteSubject(int? id)
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
        [HttpPost, ActionName("DeleteSubject")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubjectConfirmed(int id)
        {
            SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Find(id);

            if (sUBJECT_ASSIGNMENT != null)
            {
                db.SUBJECT_ASSIGNMENT.Remove(sUBJECT_ASSIGNMENT);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult CreateSubject(int id)
        {
            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name");
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "Name");
            ViewBag.SubjectTypeID = new SelectList(db.SUBJECT_TYPE_DICT, "ID", "Description");
            ViewBag.StudiesTypeID = new SelectList(db.STUDIES_TYPE_DICT, "ID", "Description");

            Session["TeacherId"] = id;

            return View(new SUBJECT_ASSIGNMENT());
        }

        // POST: Teacher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubject([Bind(Include = "ID,IsSubstitute,IsSubstituteDescription,StudentGroupID,SubjectID,SubjectTypeID, StudiesTypeID, Semester, Hours")] SUBJECT_ASSIGNMENT sUBJECT_ASSIGNMENT)
        {
            if (ModelState.IsValid)
            {
                sUBJECT_ASSIGNMENT.TeacherID = (int)Session["TeacherID"];
                db.SUBJECT_ASSIGNMENT.Add(sUBJECT_ASSIGNMENT);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.StudentGroupID = new SelectList(db.STUDENT_GROUP, "ID", "Name", sUBJECT_ASSIGNMENT.StudentGroupID);
            ViewBag.SubjectID = new SelectList(db.SUBJECT, "ID", "Name", sUBJECT_ASSIGNMENT.SubjectID);
            ViewBag.SubjectTypeID = new SelectList(db.SUBJECT_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.SubjectTypeID);
            ViewBag.StudiesTypeID = new SelectList(db.STUDIES_TYPE_DICT, "ID", "Description", sUBJECT_ASSIGNMENT.StudiesTypeID);

            return View(sUBJECT_ASSIGNMENT);
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
