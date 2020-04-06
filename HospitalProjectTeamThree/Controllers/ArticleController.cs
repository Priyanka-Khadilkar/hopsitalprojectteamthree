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
        public ActionResult List(string articlesearchkey, int pagenum = 0)
        {
            //can we access the search key?
            //Debug.WriteLine("The search key is "+articlesearchkey);



            string query = "Select * from Articles"; //order by is needed for offset
            //easier in a list.. we don't know how many more we'll add yet
            List<SqlParameter> sqlparams = new List<SqlParameter>();

            if (articlesearchkey != "")
            {
                //modify the query to include the search key
                query = query + " where ArticleTitle like @searchkey";
                sqlparams.Add(new SqlParameter("@searchkey", "%" + articlesearchkey + "%"));
                //Debug.WriteLine("The query is "+ query);
            }
            List<Crisis> crises = db.Crisiss.SqlQuery("select * from Crises").ToList();

            List<Article> articles = db.Articles.SqlQuery(query, sqlparams.ToArray()).ToList();
            // Code reference - Christine Bittle
            //Start of Pagination Algorithm (Raw MSSQL)
            int perpage = 3;
            int artcount = articles.Count();
            int maxpage = (int)Math.Ceiling((decimal)artcount / perpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(perpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                List<SqlParameter> newparams = new List<SqlParameter>();

                if (articlesearchkey != "")
                {
                    newparams.Add(new SqlParameter("@searchkey", "%" + articlesearchkey + "%"));
                    ViewData["articlesearchkey"] = articlesearchkey;
                }
                newparams.Add(new SqlParameter("@start", start));
                newparams.Add(new SqlParameter("@perpage", perpage));
                string pagedquery = query + " order by ArticleId offset @start rows fetch first @perpage rows only ";
                //Debug.WriteLine(pagedquery);
                //Debug.WriteLine("offset " + start);
                //Debug.WriteLine("fetch first " + perpage);
                articles = db.Articles.SqlQuery(pagedquery, newparams.ToArray()).ToList();
            }
            //End of Pagination Algorithm
            //Begin ShowCrisis ViewModel
            ShowCrisis viewmodel = new ShowCrisis();
            viewmodel.listcrises = crises;
            viewmodel.articles = articles;
            //End ShowCrisis ViewModel
            return View(viewmodel);

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

        public ActionResult CreateArticle(string ArticleAuthor, string ArticleTitle, string ArticleContent, int CrisisId)
        {
            DateTime DatePosted = DateTime.Now;

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