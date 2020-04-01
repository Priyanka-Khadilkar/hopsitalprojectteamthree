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
using System.IO;


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
            List<GetWellSoonCard> cards = db.GetWellSoonCards.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the cards");
            return View(cards);
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Add(string message, string HasPic, string PicExt, string PatientName, string PatientNumber, string RoomNumber)
        {
            string query = "Insert into GetWellSoonCards (message, HasPic, PicExt, PatientName, PatientEmail , RoomNumber) values (@message, @HasPic, @PicExt, @PatientName, @RoomNumber)";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@message", message);
            sqlparams[1] = new SqlParameter("@HasPic", HasPic);
            sqlparams[2] = new SqlParameter("@PicExt", PicExt);
            sqlparams[3] = new SqlParameter("@PatientName", PatientName);
            sqlparams[4] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[5] = new SqlParameter("@RoomNumber", RoomNumber);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
         ;
        }
    }
}