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
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public ArticleController() { }
        // GET: Article
        public ActionResult Index()
        {
            //check if admin

            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Editor"))
                {
                    return RedirectToAction("ListAdm");
                }
                else
                {
                    return RedirectToAction("List");
                }

            }
            else
            {
                return View();
            }
            //end check if admin

        }
    
        public ActionResult List(string articlesearchkey, int pagenum = 0)
        {

            string query = "Select * from Articles"; 
            
            List<SqlParameter> sqlparams = new List<SqlParameter>();

            if (articlesearchkey != "")
            {
                
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
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult ListAdm(string articlesearchkey, int pagenum = 0)
        {

            string query = "Select * from Articles";

            List<SqlParameter> sqlparams = new List<SqlParameter>();

            if (articlesearchkey != "")
            {

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
        public ActionResult Add()
        {
            List<Crisis> crises = db.Crisiss.SqlQuery("select * from Crises").ToList();
            return View(crises);
        }
        [HttpPost]

        public ActionResult Add(string ArticleAuthor, string ArticleTitle, string ArticleContent, int CrisisId)
        {
            DateTime DatePosted = DateTime.Now;

            //Debug.WriteLine("Value of variables are " + ArticleAuthor + ArticleTitle + ArticleContent + DatePosted);

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
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Update(int id)
        {
            //retrieves info for a specific article
            Article selectedarticle = db.Articles.SqlQuery("select * from Articles where ArticleId = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedarticle);
        }
        [HttpPost]
        public ActionResult Update(int id, string ArticleTitle,  string ArticleAuthor, string ArticleContent)
        {

            //Debug.WriteLine("I am trying to display variables" + id + ArticleTitle + ArticleAuthor + ArticleContent );

            // updates the record on the submission
            string query = "update Articles SET  ArticleTitle=@ArticleTitle, ArticleAuthor=@ArticleAuthor, ArticleContent=@ArticleContent where ArticleId=@id";
            SqlParameter[] sqlparams = new SqlParameter[4];

            sqlparams[0] = new SqlParameter("@ArticleTitle", ArticleTitle);
            sqlparams[1] = new SqlParameter("@id", id);
            sqlparams[2] = new SqlParameter("@ArticleAuthor", ArticleAuthor);
            sqlparams[3] = new SqlParameter("@ArticleContent", ArticleContent);
            


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Delete(int id)
        {
                        
            //  delete article given id
            string query = "delete  from Articles where ArticleId=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);


            return RedirectToAction("List");
        }
       
        public ActionResult Admin()
        {
            return View();
        }
    }
}