using System;
using System.Collections.Generic;
using System.Data;
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

namespace HospitalProjectTeamThree.Controllers
{
    public class ArticleController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Article
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            string query = "Select * from Articles ";
            List<Article> articles = db.Articles.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(articles);
        }
        public ActionResult ViewArticle(int? id)
        {
            // gets one article using id passed from list page
            List<Article> article = db.Articles.SqlQuery("select * from Articles where ArticleId=@ArticleId", new SqlParameter("@ArticleId", id)).ToList();
            Debug.WriteLine("Checking if getting id :" + id);
            return View(article);
        }
        public ActionResult CreateArticle()
        {
            List<Crisis> crises = db.Crisiss.SqlQuery("select * from Crises").ToList();
            return View(crises);
        }
        [HttpPost]

        public ActionResult CreateArticle(string ArticleAuthor, string ArticleTitle, string ArticleContent, DateTime DatePosted, int CrisisId)
        {

            Debug.WriteLine("Value of variables are " + ArticleAuthor + ArticleTitle + ArticleContent + DatePosted);

            string query = "insert into Articles (ArticleTitle, ArticleAuthor, ArticleContent, DatePosted, Crisis_CrisisId) values (@ArticleTitle, @ArticleAuthor, @ArticleContent, @DatePosted, @Crisis_CrisisId)";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@ArticleTitle", ArticleTitle);
            sqlparams[1] = new SqlParameter("@ArticleAuthor", ArticleAuthor);
            sqlparams[2] = new SqlParameter("@ArticleContent", ArticleContent);
            sqlparams[3] = new SqlParameter("@Crisis_CrisisId", CrisisId);
            sqlparams[4] = new SqlParameter("@DatePosted", DatePosted);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}