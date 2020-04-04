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
using System.Collections.Generic;
using HospitalProjectTeamThree.Data;
using System.Diagnostics;
using System.Data.SqlClient;

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
            List<Crisis> crises = db.Crises.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(crises);
            
        }
        public ActionResult CreateCrisis()
        {
            return View();
        }
        [HttpPost]

        public ActionResult CreateCrisis(string CrisisName, DateTime CrisisStarted, DateTime CrisisFinished, string CrisisDesc)
        {

            Debug.WriteLine("Value of variables are " + CrisisName + CrisisStarted + CrisisFinished + CrisisDesc);
           
            string query = "insert into Crises (CrisisName, CrisisStrated, CrisisFinished, CrisisDesc) values (@CrisisName, @CrisisStrated, @CrisisFinished,@CrisisDesc )";
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
            //retrieves info for a specific vehicle type
            Crisis selectedcrisis = db.Crises.SqlQuery("select * from Crises where CrisisId = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedcrisis);
        }
        [HttpPost]
        public ActionResult UpdateCrisis(int id, string CrisisName, DateTime CrisisStarted, DateTime CrisisFinished, string CrisisDesc)
        {

            Debug.WriteLine("I am trying to display variables" + id + CrisisName + CrisisStarted + CrisisFinished + CrisisDesc);

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
            string query = "delete from Crises where CrisisId=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("CrisisList");
        }
    }
}