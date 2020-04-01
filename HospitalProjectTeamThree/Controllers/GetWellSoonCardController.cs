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
//using HospitalProjectTeamThree.Models.ViewModels;
using System.Diagnostics;


namespace HospitalProjectTeamThree.Controllers
{
    public class GetWellSoonCardController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: GetWellSoonCard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            string query = "Select * from GetWellSoonCards";
            //db.GetWellSoonCards.SqlQuery(query);
            List<GetWellSoonCard> cards = db.GetWellSoonCards.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the comics");
            return View(cards);
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Add(string message)
        {
            string query = "Insert into GetWellSoonCards (message) values (@message)";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@message", message);
            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
         ;
        }
    }
}