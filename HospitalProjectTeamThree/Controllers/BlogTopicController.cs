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
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace HospitalProjectTeamThree.Controllers
{
    public class BlogTopicController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();

        public BlogTopicController() { }
        // Blog Topic Pages will be accessed through the Doctors Blog Pages
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult List(string topicsearchkey, int pagenum = 0)
        {
            // Admins and Editors will be able to see the full list and the option to add, delete or edit.
            List<BlogTopic> topics = db
                .Topics
                .Where(b => (topicsearchkey != null) ? b.TopicName.Contains(topicsearchkey) : true)
                .ToList();

            //we start the pagination, we include the searchkey in case there are more than 5 topics that contain that word in the name
            int tperpage = 5;
            int topiccount = topics.Count();
            int maxpage = (int)Math.Ceiling((decimal)topiccount / tperpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(tperpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                topics = db.Topics
                    .Where(t => (topicsearchkey != null) ? t.TopicName.Contains(topicsearchkey) : true)
                    .OrderBy(t => t.TopicId)
                    .Skip(start)
                    .Take(tperpage)
                    .ToList();
            } else
            {
                // if there are less than the limit per page show page 1 of 1
                ViewData["pagesummary"] = "1 of 1";
            }
            //Debug.WriteLine("Iam trying to list all the topics");
            return View(topics);
        }

        public ActionResult PublicList(string topicsearchkey, int pagenum = 0)
        {
            //guests and registered users can see the full list of topics
            List<BlogTopic> topics = db
                .Topics
                .Where(b => (topicsearchkey != null) ? b.TopicName.Contains(topicsearchkey) : true)
                .ToList();

            //we start the pagination, we include the searchkey in case there are more than 5 topics that contain that word in the name
            int tperpage = 5;
            int topiccount = topics.Count();
            int maxpage = (int)Math.Ceiling((decimal)topiccount / tperpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(tperpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                topics = db.Topics
                    .Where(t => (topicsearchkey != null) ? t.TopicName.Contains(topicsearchkey) : true)
                    .OrderBy(t => t.TopicId)
                    .Skip(start)
                    .Take(tperpage)
                    .ToList();
            }
            else
            {
                // if there are less than the limit per page show page 1 of 1
                ViewData["pagesummary"] = "1 of 1";
            }
            //Debug.WriteLine("Iam trying to list all the topics");
            return View(topics);
        }

        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            BlogTopic blogtopic = db.Topics.SqlQuery("select * from BlogTopics where TopicId=@TopicId", new SqlParameter("@TopicId", id)).FirstOrDefault();
            if (blogtopic == null)
            {
                return HttpNotFound();
            }

            string blogs_query = "Select * from DoctorsBlogs inner join BlogTopicDoctorsBlogs on DoctorsBlogs.BlogId = BlogTopicDoctorsBlogs.DoctorsBlog_BlogId where BlogTopicDoctorsBlogs.BlogTopic_TopicId=@TopicId";
            var t_parameter = new SqlParameter("@TopicId", id);
            List<DoctorsBlog> blogs = db.DoctorsBlogs.SqlQuery(blogs_query, t_parameter).ToList();


            // We use the AddBlogTopic viewmodel so that we can show the blogs which have that specific topic
            AddBlogTopic viewmodel = new AddBlogTopic();
            viewmodel.Topics = blogtopic;
            viewmodel.Blogs = blogs;

            return View(viewmodel);
        }
        public ActionResult PublicShow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            BlogTopic blogtopic = db.Topics.SqlQuery("select * from BlogTopics where TopicId=@TopicId", new SqlParameter("@TopicId", id)).FirstOrDefault();
            if (blogtopic == null)
            {
                return HttpNotFound();
            }

            string blogs_query = "Select * from DoctorsBlogs inner join BlogTopicDoctorsBlogs on DoctorsBlogs.BlogId = BlogTopicDoctorsBlogs.DoctorsBlog_BlogId where BlogTopicDoctorsBlogs.BlogTopic_TopicId=@TopicId";
            var t_parameter = new SqlParameter("@TopicId", id);
            List<DoctorsBlog> blogs = db.DoctorsBlogs.SqlQuery(blogs_query, t_parameter).ToList();


            // We use the AddBlogTopic viewmodel so that we can show the blogs which have that specific topic
            AddBlogTopic viewmodel = new AddBlogTopic();
            viewmodel.Topics = blogtopic;
            viewmodel.Blogs = blogs;

            return View(viewmodel);
        }

        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Add()
        {

            return View();

        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public ActionResult Add(string TopicName)
        {
            //Debug.WriteLine("Want to create a new blog topic named " + TopicName ) ;

            // We create the query to insert the values we will get into the database
            string query = "insert into BlogTopics (TopicName) values (@TopicName)";
            SqlParameter[] sqlparams = new SqlParameter[1];

            sqlparams[0] = new SqlParameter("@TopicName", TopicName);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            // Once added we go back to the list of topics
           
            return RedirectToAction("List");

        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Update(int id)
        {
            //need information about a blog topic to update
            BlogTopic selectedtopic = db.Topics.SqlQuery("select * from BlogTopics where TopicId=@TopicId", new SqlParameter("@TopicId", id)).FirstOrDefault();

            return View(selectedtopic);
        }
        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public ActionResult Update(string TopicName, int id)
        {

            // Debug.WriteLine("I am trying to edit a topics name to "+ TopicName +" );
            // query to update information in the database
            string query = "update BlogTopics set TopicName=@TopicName where TopicId=@TopicId";

            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@TopicName", TopicName);
            sqlparams[1] = new SqlParameter("@TopicId", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Delete(int id)
        {
            //We show a specific delete confirmation with the topic name so the user can confirm or cancel
            BlogTopic topic = db.Topics.SqlQuery("select * from BlogTopics where TopicId=@TopicId", new SqlParameter("@TopicId", id)).FirstOrDefault();

            return View(topic);
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult DeleteF(int id)
        {
            //Once the user confirms they want to delete the topic
            //We run the query to delete it
            string query = "delete from BlogTopics where TopicId=@TopicId";
            SqlParameter sqlparam = new SqlParameter("@TopicId", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            //Once deleted it takes the user back to the list of topics
            return RedirectToAction("List");
        }
        public BlogTopicController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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