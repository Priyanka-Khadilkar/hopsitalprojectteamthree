using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using System.Collections.Generic;
using HospitalProjectTeamThree.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;
using System.Data.Entity;

namespace HospitalProjectTeamThree.Controllers
{

    public class CrisisController : Controller
    {

    private ApplicationSignInManager _signInManager;
    private ApplicationUserManager _userManager;
    private ApplicationRoleManager _roleManager;
    public CrisisController() { }
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Crisis
        public ActionResult Index()
        {
            //Redirects the user to appropriate page, based on their credentials
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
        public ActionResult List()
        {        
            //Makes list of all crisis
            string query = "Select * from Crises ";
            List<Crisis> crises = db.Crisiss.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(crises);

        }

        [Authorize(Roles = "Admin, Editor")]
        public ActionResult ListAdm()
        {
            //List of all crisis for Administrative view           
            string query = "Select * from Crises ";
            List<Crisis> crises = db.Crisiss.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(crises);

        }
        public ActionResult ViewCrisis( int? id,  string articlesearchkey,int pagenum= 0)
        {
            //Snow all articles related to one selected Crisis            
            //Debug.WriteLine("Crisis Id is:" + id);
            //get all info about specific crisis
            Crisis Crisis = db.Crisiss.SqlQuery("select * from Crises where CrisisId=@Id", new SqlParameter("@Id", id)).FirstOrDefault();


            //list all articles related to specific crisis
            string query = "select * from Articles inner join Crises on Articles.Crisis_CrisisId=Crises.CrisisId where Crisis_CrisisId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Article> Articles = db.Articles.SqlQuery(query, param).ToList();
            //begin 

            // Code reference - Christine Bittle

            //Start of Pagination Algorithm (Raw MSSQL)
            int perpage = 3;
            int artcount = Articles.Count();
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
                newparams.Add(new SqlParameter("@id", id));
                newparams.Add(new SqlParameter("@start", start));
                newparams.Add(new SqlParameter("@perpage", perpage));
                string pagedquery = query + " order by Crisis_CrisisId offset @start rows fetch first @perpage rows only ";
                //Debug.WriteLine(pagedquery);
                //Debug.WriteLine("offset " + start);
                //Debug.WriteLine("fetch first " + perpage);
                Articles = db.Articles.SqlQuery(pagedquery, newparams.ToArray()).ToList();
            }
            //End of Pagination Algorithm
            
                       
            ShowCrisis viewmodel = new ShowCrisis();
            viewmodel.crises = Crisis;
            viewmodel.articles = Articles;
           
            return View(viewmodel);
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
       
        public ActionResult Add(string CrisisName, string CrisisFinished, string CrisisDesc)
        {
            //Allows to add crisis
            //add current date stamp as begigning of crisis
            DateTime CrisisStarted = DateTime.Now;
            //add word "No" to allow for further logic to create alert on main page
            if (CrisisFinished == "")
            {
                CrisisFinished = "No";
               
            }
            //Debug.WriteLine("Value of variables are " + CrisisName + CrisisStarted + CrisisFinished + CrisisDesc);
            
                string query = "insert into Crises (CrisisName, CrisisStrated,CrisisFinished, CrisisDesc) values (@CrisisName, @CrisisStrated,@CrisisFinished,@CrisisDesc )";
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@CrisisName", CrisisName);
                sqlparams[1] = new SqlParameter("@CrisisStrated", CrisisStarted);
                sqlparams[2] = new SqlParameter("@CrisisFinished", CrisisFinished);
                sqlparams[3] = new SqlParameter("@CrisisDesc", CrisisDesc);
                db.Database.ExecuteSqlCommand(query, sqlparams);
                     
            
            return RedirectToAction("CrisisList");
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Update(int id)
        {
            
            //retrieves info for a specific crisis
            Crisis selectedcrisis = db.Crisiss.SqlQuery("select * from Crises where CrisisId = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedcrisis);
        }
        [HttpPost]
        public ActionResult Update(int id, string CrisisName, DateTime CrisisStarted, string CrisisFinished, string CrisisDesc)
        {
            //Allows to update crisis, including updating CrisisFinished, which will remove alert from main page
            //Debug.WriteLine("I am trying to display variables" + id + CrisisName + CrisisStarted + CrisisFinished + CrisisDesc);

            // updates the record on the submission
            string query = "update Crises SET  CrisisName=@CrisisName, CrisisStrated=@CrisisStrated, CrisisFinished=@CrisisFinished, CrisisDesc=@CrisisDesc where CrisisId=@id";
            SqlParameter[] sqlparams = new SqlParameter[5];

            sqlparams[0] = new SqlParameter("@CrisisName", CrisisName);
            sqlparams[1] = new SqlParameter("@id", id);
            sqlparams[2] = new SqlParameter("@CrisisStrated", CrisisStarted);
            sqlparams[3] = new SqlParameter("@CrisisFinished", CrisisFinished);
            sqlparams[4] = new SqlParameter("@CrisisDesc", CrisisDesc);


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        public ActionResult Delete(int id)
        {
            //Deletes the record from the database
            //https://www.mysqltutorial.org/mysql-delete-join/
            //string query = "delete Articles, Crises from Articles Inner Join Crises on Crises.CrisisId = Articles.Crisis_CrisisId where Crisis_CrisisID=@id";
            
            //I  will delete all articles related to the Crisis with id provided first
            string query1 = "delete from articles where Crisis_CrisisId=@id";
            SqlParameter sqlparam1 = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query1, sqlparam1);


            // and then delete crisis itself
            string query = "delete  from Crises where CrisisID=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);

    
            return RedirectToAction("List");
        }

    }
}