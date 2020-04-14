using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using System.Data.SqlClient;
using System.Net;
using System.Diagnostics;

namespace HospitalProjectTeamThree.Controllers
{
    public class FeedbackController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Feedback
        [Authorize(Roles = "Registered User")]
        [HttpPost]
        public ActionResult Add(string DoctorName, string Compliment, string Complain, string Suggestions, string Others, string StaffKnowledge, string Hygiene, string WaitTime, string Comments)
        {
            //Debug.WriteLine("Compliment");
            //Debug.WriteLine(Compliment);
            //Debug.WriteLine("Complain");
            //Debug.WriteLine(Complain);
            string query = "insert into Feedbacks (DoctorName, FeedbackType, StaffKnowledge, Hygiene, WaitTime, Comments,UserId) values (@DoctorName, @FeedbackType, @StaffKnowledge, @Hygiene, @WaitTime, @Comments,@UserId)";
            SqlParameter[] sqlparams = new SqlParameter[7];
            sqlparams[0] = new SqlParameter("@DoctorName", DoctorName);
            List<string> feedbackTypes = new List<String>();
            if (Compliment == "on")
            {
                feedbackTypes.Add("Compliment");
            }
            if (Complain == "on")
            {
                feedbackTypes.Add("Complain");
            }
            if (Suggestions == "on")
            {
                feedbackTypes.Add("Suggestions");
            }
            if (Others == "on")
            {
                feedbackTypes.Add("Others");
            }
            string feedback = String.Join(", ", feedbackTypes);
            sqlparams[1] = new SqlParameter("@FeedbackType", feedback);
            sqlparams[2] = new SqlParameter("@StaffKnowledge", StaffKnowledge);
            sqlparams[3] = new SqlParameter("@Hygiene", Hygiene);
            sqlparams[4] = new SqlParameter("@WaitTime", WaitTime);
            sqlparams[5] = new SqlParameter("@Comments", Comments);
            var UserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            sqlparams[6] = new SqlParameter("@UserId", UserId);
            db.Database.ExecuteSqlCommand(query, sqlparams);


            return RedirectToAction("List");
        }


        public ActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Registered User")]
        public ActionResult List()

        {
            var UserId = User.Identity.GetUserId();
            List<Feedback> feedback = db.Feedbacks.SqlQuery("Select * from Feedbacks where UserId = @userId", new SqlParameter("@userId", UserId)).ToList();
            if (User.IsInRole("Admin"))
            {
                feedback = db.Feedbacks.SqlQuery("Select * from Feedbacks").ToList();
            }
            return View(feedback);
        }

       
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Feedback feedback = db.Feedbacks.SqlQuery("Select * from Feedbacks where FeedbackFormId=@FeedbackFormId", new SqlParameter("@FeedbackFormId", id)).FirstOrDefault();
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }
      
        public ActionResult ConfirmDelete(int id)
        {
            string query = "delete from feedbacks where FeedbackFormId=@id";
            SqlParameter sqlparams = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
       
        public ActionResult Delete(int id)
        {
            string query = "select * from feedbacks where FeedbackFormId = @id";
            SqlParameter sqlparams = new SqlParameter("@id", id);
            Feedback selectedfeedback = db.Feedbacks.SqlQuery(query, sqlparams).FirstOrDefault();

            return View(selectedfeedback);
        }

    }

}