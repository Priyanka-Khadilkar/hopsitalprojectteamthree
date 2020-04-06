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
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Crisis
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CrisisList()
        {
            string query = "Select * from Crises ";
            List<Crisis> crises = db.Crisiss.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(crises);

        }
        public ActionResult ViewCrisis(int? id)
        {


            //get all info about specific crisis
            Crisis Crisis = db.Crisiss.SqlQuery("select * from Crises where CrisisId=@CrisisId", new SqlParameter("@CrisisId", id)).FirstOrDefault();


            //list all articles related to specific crisis
            string query = "select * from Articles inner join Crises on Articles.Crisis_CrisisId=Crises.CrisisId where Crisis_CrisisId = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Article> Articles = db.Articles.SqlQuery(query, param).ToList();


            ShowCrisis viewmodel = new ShowCrisis();
            viewmodel.crises = Crisis;
            viewmodel.articles = Articles;



            return View(viewmodel);
        }
        public ActionResult CreateCrisis()
        {
            return View();
        }
        [HttpPost]

        public ActionResult CreateCrisis(string CrisisName, string CrisisFinished, string CrisisDesc)
        {
            DateTime CrisisStarted = DateTime.Now;
            if (CrisisFinished == "")
            {
                CrisisFinished = "No";
               
            }
            Debug.WriteLine("Value of variables are " + CrisisName + CrisisStarted + CrisisFinished + CrisisDesc);
            
                string query = "insert into Crises (CrisisName, CrisisStrated,CrisisFinished, CrisisDesc) values (@CrisisName, @CrisisStrated,@CrisisFinished,@CrisisDesc )";
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@CrisisName", CrisisName);
                sqlparams[1] = new SqlParameter("@CrisisStrated", CrisisStarted);
                sqlparams[2] = new SqlParameter("@CrisisFinished", CrisisFinished);
                sqlparams[3] = new SqlParameter("@CrisisDesc", CrisisDesc);
                db.Database.ExecuteSqlCommand(query, sqlparams);

            
            
            return RedirectToAction("CrisisList");
        }

        public ActionResult UpdateCrisis(int id)
        {
            //retrieves info for a specific crisis
            Crisis selectedcrisis = db.Crisiss.SqlQuery("select * from Crises where CrisisId = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedcrisis);
        }
        [HttpPost]
        public ActionResult UpdateCrisis(int id, string CrisisName, DateTime CrisisStarted, string CrisisFinished, string CrisisDesc)
        {

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
            return RedirectToAction("CrisisList");
        }
        public ActionResult DeleteCrisis(int id)
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

            //Need to delete from Articles where CrisisId = id
            //string query2 = "delete from Articles where Crisis_CrisiId=@id";
            //SqlParameter sqlparam2 = new SqlParameter("@id", id);
            //db.Database.ExecuteSqlCommand(query, sqlparam);

            return RedirectToAction("CrisisList");
        }
    }
}