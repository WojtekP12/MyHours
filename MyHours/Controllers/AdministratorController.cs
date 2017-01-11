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
    public class AdministratorController : Controller
    {
        private TAM_DBEntities db = new TAM_DBEntities();

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
            int usedTime;
            int freeTime;

            var sUBJECT_ASSIGNMENT = db.SUBJECT_ASSIGNMENT.Include(s => s.STUDENT_GROUP).Include(s => s.SUBJECT).Include(s => s.TEACHER).Include(s => s.SUBJECT_TYPE_DICT).Include(s => s.STUDIES_TYPE_DICT);

            var teacherID = db.USER.Where(x => x.ID == id).FirstOrDefault().TeacherID;

            totalTime = db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().AssignedHours;
            var subjectAssignment = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).FirstOrDefault();

            if(subjectAssignment == null)
            {
                usedTime = 0;
            }
            else
            {
                usedTime = sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).Sum(x => x.Hours);
            }
            
            freeTime = totalTime - usedTime;

            if (freeTime < 0)
            {
                freeTime = 0;
            }

            ViewBag.TotalTime = totalTime;
            ViewBag.UsedTime = usedTime;
            ViewBag.FreeTime = freeTime;
            ViewBag.Teacher = db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().TeacherStatus + " " + db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().FirstName + " " + db.TEACHER.Where(x => x.ID == teacherID).FirstOrDefault().SecondName;

            return View(sUBJECT_ASSIGNMENT.Where(x => x.TeacherID == teacherID).ToList());
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
