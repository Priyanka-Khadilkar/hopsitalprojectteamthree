using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using System.Diagnostics;
//using System.IO;

namespace HospitalProjectTeamThree.Controllers
{
    public class DoctorsBlogController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public ActionResult List()
        {
            string query = "Select * from DoctorsBlogs";
            List<DoctorsBlog> blogs = db.DoctorsBlogs.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the blogs");
            return View(blogs);
        }
        public ActionResult Add()
        {
            // On the new blog entry we can select a topic
            List<BlogTopic> Topics = db.Topics.SqlQuery("select * from BlogTopics").ToList();

            //We use the AddParkGuest viewmodel to show the guessts and parks and be able to add them to the booking.
            AddBlogTopic AddBlogTopicViewModel = new AddBlogTopic();
            AddBlogTopicViewModel.BlogTopics = Topics;

            return View(AddBlogTopicViewModel);

        }
        [HttpPost]
        public ActionResult Add(string BlogTitle, string BlogContent, string BlogSource, int TopicId)
        {
            //Debug.WriteLine("Want to create a blog entry with a title of " + BlogTitle ) ;

            // We create the query to insert the values we will get into the database
            string query = "insert into DoctorsBlogs (BlogTitle, BlogContent, BlogSource, TopicId) values (@Date,@Cost,@ParkID)";
            SqlParameter[] sqlparams = new SqlParameter[3];

            sqlparams[0] = new SqlParameter("@Date", Date);
            sqlparams[1] = new SqlParameter("@Cost", Cost);
            sqlparams[2] = new SqlParameter("@ParkID", ParkID);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            // Once added we go back to the list of bookings where our new booking will be
            return RedirectToAction("List");
        }

    }
}