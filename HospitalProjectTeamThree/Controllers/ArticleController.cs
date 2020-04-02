using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;

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
            Debug.WriteLine("Checking if getting id :" +id);
            return View(article);
        }
        public ActionResult CreateArticle()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
    }
}