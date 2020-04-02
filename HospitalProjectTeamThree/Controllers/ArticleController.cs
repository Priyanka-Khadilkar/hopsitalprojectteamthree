using System;
using System.Collections.Generic;
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
    }
}