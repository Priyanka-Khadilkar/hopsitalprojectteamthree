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
        // GET details about one blog entry
        
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

            // We use the AddParkGuest viewmodel so that we can show the guests that are on that booking and also so that we can see the dropdown list of guests
            // and add a guest to a booking if we want to
            AddBlogTopic viewmodel = new AddBlogTopic();
            viewmodel.Blog = doctorsblog;
            viewmodel.BlogTopics = usedtopics;
            viewmodel.Add_Topic = AllTopics;

            return View(viewmodel);

        }
        public ActionResult Add()
        {
            // On the new blog entry we can select a topic
            List<BlogTopic> Topics = db.Topics.SqlQuery("select * from BlogTopics").ToList();

            //We use the AddBlogTopic viewmodel to show the topics and be able to add them to the blog entry.
            AddBlogTopic AddBlogTopicViewModel = new AddBlogTopic();
            AddBlogTopicViewModel.BlogTopics = Topics;

            return View(AddBlogTopicViewModel);

        }
        [HttpPost]
        public ActionResult Add(string BlogTitle, string BlogContent, string BlogSource)
        {
            //Debug.WriteLine("Want to create a blog entry with a title of " + BlogTitle ) ;
            DateTime BlogDate = DateTime.Now;
            // We create the query to insert the values we will get into the database
            string query = "insert into DoctorsBlogs (BlogTitle, BlogContent, BlogSource, BlogDate) values (@BlogTitle,@BlogContent,@BlogSource, @BlogDate)";
            SqlParameter[] sqlparams = new SqlParameter[4];

            sqlparams[0] = new SqlParameter("@BlogTitle", BlogTitle);
            sqlparams[1] = new SqlParameter("@BlogContent", BlogContent);
            sqlparams[2] = new SqlParameter("@BlogSource", BlogSource);
            sqlparams[3] = new SqlParameter("@BlogDate", BlogDate);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            // Once added we go back to the list of blogs
            return RedirectToAction("List");
        }
        
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
            //It takes us back to the list where we can see any updates
            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            //We show a specific delete confirmation with the blog entry information
            DoctorsBlog blog = db.DoctorsBlogs.SqlQuery("select * from DoctorsBlogs where BlogId=@BlogId", new SqlParameter("@BlogId", id)).FirstOrDefault();

            return View(blog);
        }

        public ActionResult DeleteF(int id)
        {
            //Once the user confirms they want to delete the blog entry
            //We run the query to delete the blog entry
            string query = "delete from DoctorsBlogs where BlogId=@BlogId";
            SqlParameter sqlparam = new SqlParameter("@BlogId", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            return RedirectToAction("List");
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