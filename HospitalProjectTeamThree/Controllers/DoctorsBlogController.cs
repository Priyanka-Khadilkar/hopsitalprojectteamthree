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
    public class DoctorsBlogController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();

        public DoctorsBlogController() { }
        public ActionResult Index()
        {
            // Depending on the user that logs in they will be sent to the doctors blog list,
            // Admin will be sent to the list will all the entries
            // Editor will be sent to their list of entries
            // Registered Users will be sent to the list of all entries but with no access to delete, update or add
            // Guests will also be redirected to the public list
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("List");
                }
                else if (User.IsInRole("Editor"))
                {
                    return RedirectToAction("DoctorPersonalList");
                }
                else
                {
                    return RedirectToAction("PublicList");
                }

            }
            else
            {
                return RedirectToAction("PublicList");
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult List(string blogsearchkey, int pagenum = 0)
        {
            List<DoctorsBlog> blogs = db
                .DoctorsBlogs
                .Where(b => (blogsearchkey != null) ? b.BlogTitle.Contains(blogsearchkey) : true)
                .ToList();

            //we start the pagination, we include the searchkey in case there are more than 5 blogs that contain that word in the title
            int bperpage = 5;
            int blogcount = blogs.Count();
            int maxpage = (int)Math.Ceiling((decimal)blogcount / bperpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(bperpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                blogs = db.DoctorsBlogs
                    .Where(b => (blogsearchkey != null) ? b.BlogTitle.Contains(blogsearchkey) : true)
                    .OrderBy(b => b.BlogId)
                    .Skip(start)
                    .Take(bperpage)
                    .ToList();
            } else
            {
                // if there are less than the limit per page show page 1 of 1
                ViewData["pagesummary"] = "1 of 1";
            }

            //Debug.WriteLine("Iam trying to list all the blogs");
            return View(blogs);
        }
        [Authorize(Roles = "Editor")]
        public ActionResult DoctorPersonalList()
        {
            // if an editor(Doctor) is logged in they will only see their blog entries as a list
            //Debug.WriteLine("Iam trying to list all my blogs");

            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);


            string query = "Select * from DoctorsBlogs where User_Id=@userId";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@userId", userId);

            List<DoctorsBlog> Blogs = db.DoctorsBlogs.SqlQuery(query, sqlparams).ToList();

            DoctorPersonalBlogList viewModel = new DoctorPersonalBlogList();
            viewModel.Blog = Blogs;
            viewModel.User = currentUser;
            // we use the view model so that we can pull only the blogs that user made

            return View(viewModel);
        }
        public ActionResult PublicList(string blogsearchkey, int pagenum = 0)
        {
            List<DoctorsBlog> blogs = db
                .DoctorsBlogs
                .Where(b => (blogsearchkey != null) ? b.BlogTitle.Contains(blogsearchkey) : true)
                .ToList();

            //we start the pagination, we include the searchkey in case there are more than 5 blogs that contain that word in the title
            int bperpage = 5;
            int blogcount = blogs.Count();
            int maxpage = (int)Math.Ceiling((decimal)blogcount / bperpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(bperpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                blogs = db.DoctorsBlogs
                    .Where(b => (blogsearchkey != null) ? b.BlogTitle.Contains(blogsearchkey) : true)
                    .OrderBy(b => b.BlogId)
                    .Skip(start)
                    .Take(bperpage)
                    .ToList();
            } else
            {
                // if there are less than the limit per page show page 1 of 1
                ViewData["pagesummary"] = "1 of 1";
            }

            //Debug.WriteLine("Iam trying to list all the blogs");
            return View(blogs);
        }
        // GET details about one blog entry
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            DoctorsBlog doctorsblog = db.DoctorsBlogs.SqlQuery("select * from DoctorsBlogs where BlogId=@BlogId", new SqlParameter("@BlogId", id)).FirstOrDefault();
            if (doctorsblog == null)
            {
                return HttpNotFound();
            }

            string topic_query = "select * from BlogTopics inner join BlogTopicDoctorsBlogs on BlogTopics.TopicId = BlogTopicDoctorsBlogs.BlogTopic_TopicId where BlogTopicDoctorsBlogs.DoctorsBlog_BlogId=@BlogId";
            var t_parameter = new SqlParameter("@BlogId", id);
            List<BlogTopic> usedtopics = db.Topics.SqlQuery(topic_query, t_parameter).ToList();

            string all_topics_query = "select * from BlogTopics";
            List<BlogTopic> AllTopics = db.Topics.SqlQuery(all_topics_query).ToList();

            // We use the AddBlogTopic viewmodel so that we can show the topics that are on that blog post and also so that we can see the dropdown list of topics
            // so that the user can add a topic
            AddBlogTopic viewmodel = new AddBlogTopic();
            viewmodel.Blog = doctorsblog;
            viewmodel.BlogTopics = usedtopics;
            viewmodel.Add_Topic = AllTopics;

            return View(viewmodel);

        }
        public ActionResult PublicShow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            DoctorsBlog doctorsblog = db.DoctorsBlogs.SqlQuery("select * from DoctorsBlogs where BlogId=@BlogId", new SqlParameter("@BlogId", id)).FirstOrDefault();
            if (doctorsblog == null)
            {
                return HttpNotFound();
            }

            string topic_query = "select * from BlogTopics inner join BlogTopicDoctorsBlogs on BlogTopics.TopicId = BlogTopicDoctorsBlogs.BlogTopic_TopicId where BlogTopicDoctorsBlogs.DoctorsBlog_BlogId=@BlogId";
            var t_parameter = new SqlParameter("@BlogId", id);
            List<BlogTopic> usedtopics = db.Topics.SqlQuery(topic_query, t_parameter).ToList();

            AddBlogTopic viewmodel = new AddBlogTopic();
            viewmodel.Blog = doctorsblog;
            viewmodel.BlogTopics = usedtopics;

            return View(viewmodel);

        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Add()
        {
            // On the new blog entry we can select a topic
            List<BlogTopic> Topics = db.Topics.SqlQuery("select * from BlogTopics").ToList();

            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            //We use the AddBlogTopic viewmodel to show the topics and be able to add them to the blog entry.
            //We also get the user who made the entry through the view model that way it also goes into their personal list.
            AddBlogTopic AddBlogTopicViewModel = new AddBlogTopic();
            AddBlogTopicViewModel.BlogTopics = Topics;
            AddBlogTopicViewModel.User = currentUser;

            return View(AddBlogTopicViewModel);

        }
        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(string BlogTitle, string BlogContent, string BlogSource, string User_Id)
        {
            //Debug.WriteLine("Want to create a blog entry with a title of " + BlogTitle ) ;
            DateTime BlogDate = DateTime.Now;
            // We create the query to insert the values we will get into the database
            string query = "insert into DoctorsBlogs (BlogTitle, BlogContent, BlogSource, BlogDate, User_Id) values (@BlogTitle,@BlogContent,@BlogSource, @BlogDate, @User_Id)";
            SqlParameter[] sqlparams = new SqlParameter[5];

            sqlparams[0] = new SqlParameter("@BlogTitle", BlogTitle);
            sqlparams[1] = new SqlParameter("@BlogContent", BlogContent);
            sqlparams[2] = new SqlParameter("@BlogSource", BlogSource);
            sqlparams[3] = new SqlParameter("@BlogDate", BlogDate);
            sqlparams[4] = new SqlParameter("@User_Id", User_Id);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            // Once added we go back to the list of blogs
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("DoctorPersonalList");
            }
        }
        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public ActionResult AddTopic(int id, int TopicId)
        {
            // Debug.WriteLine("topic id is" + TopicId + " and blog id is " + id);

            //We first check if the blog already has a tpoic, if not we get the list
            string check_query = "select * from BlogTopics inner join BlogTopicDoctorsBlogs on BlogTopicDoctorsBlogs.BlogTopic_TopicId = BlogTopics.TopicId where BlogTopic_TopicId=@TopicId and DoctorsBlog_BlogId=@BlogId";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@BlogId", id);
            check_params[1] = new SqlParameter("@TopicId", TopicId);
            List<BlogTopic> topics = db.Topics.SqlQuery(check_query, check_params).ToList();

            //Only adds a topic if they don't exist yet.
            if (topics.Count <= 0)
            {

                //query to insert the values into our bridging table between the blogs and topics
                string query = "insert into BlogTopicDoctorsBlogs (BlogTopic_TopicId, DoctorsBlog_BlogId) values (@TopicId, @BlogId)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@BlogId", id);
                sqlparams[1] = new SqlParameter("@TopicId", TopicId);

                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            return RedirectToAction("Show/" + id);

        }

        //If we want to delete a topic from a blog entry on the show page
        [HttpGet]
        public ActionResult DeleteTopic(int id, int TopicId)
        {

            //Debug.WriteLine("topic id is" + TopicId + " and blog id is " + id);

            //We run the query to delete it from the bridging table and this way we only remove the topic from the blog entry 
            //but we do not delete the topic from the topics table
            string query = "delete from BlogTopicDoctorsBlogs where BlogTopic_TopicId=@TopicId and DoctorsBlog_BlogId=@BlogId";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@TopicId", TopicId);
            sqlparams[1] = new SqlParameter("@BlogId", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/" + id);
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Update(int id)
        {
            //need information about a particular blog entry
            DoctorsBlog selectedblog = db.DoctorsBlogs.SqlQuery("select * from DoctorsBlogs where BlogId=@BlogId", new SqlParameter("@BlogId", id)).FirstOrDefault();
            string topic_query = "select * from BlogTopics inner join BlogTopicDoctorsBlogs on BlogTopics.TopicId = BlogTopicDoctorsBlogs.BlogTopic_TopicId where BlogTopicDoctorsBlogs.DoctorsBlog_BlogId=@BlogId";
            var t_parameter = new SqlParameter("@BlogId", id);
            List<BlogTopic> usedtopics = db.Topics.SqlQuery(topic_query, t_parameter).ToList();



            //We use the AddBlogTopic viewmodel to show the topics.
            AddBlogTopic AddBlogTopicViewModel = new AddBlogTopic();
            AddBlogTopicViewModel.Blog = selectedblog;
            AddBlogTopicViewModel.BlogTopics = usedtopics;


            return View(AddBlogTopicViewModel);
        }

        [Authorize(Roles = "Admin, Editor")]
        [HttpPost]
        public ActionResult Update(string BlogTitle, string BlogContent, string BlogSource, int id)
        {

            //Debug.WriteLine("I want to edit a blog entry title to "+BlogTitle );

            //We create the query to update the blog table
            string query = "update DoctorsBlogs set BlogTitle=@BlogTitle, BlogContent=@BlogContent, BlogSource=@BlogSource where BlogId=@BlogId";

            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@BlogTitle", BlogTitle);
            sqlparams[1] = new SqlParameter("@BlogContent", BlogContent);
            sqlparams[2] = new SqlParameter("@BlogSource", BlogSource);
            sqlparams[3] = new SqlParameter("@BlogId", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            //Once updated it takes an Admin back to full list of blogs and an Editor to their list of blogs.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("DoctorPersonalList");
            }
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Delete(int id)
        {
            //We show a specific delete confirmation with the blog entry information
            DoctorsBlog blog = db.DoctorsBlogs.SqlQuery("select * from DoctorsBlogs where BlogId=@BlogId", new SqlParameter("@BlogId", id)).FirstOrDefault();

            return View(blog);
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult DeleteF(int id)
        {
            //Once the user confirms they want to delete the blog entry
            //We run the query to delete the blog entry
            string query = "delete from DoctorsBlogs where BlogId=@BlogId";
            SqlParameter sqlparam = new SqlParameter("@BlogId", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            //Once deleted it takes an Admin back to full list of blogs and an Editor to their list of blogs.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("DoctorPersonalList");
            }
        }

        public DoctorsBlogController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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