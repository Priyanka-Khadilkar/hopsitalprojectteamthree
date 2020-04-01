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
//using System.IO;


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
        [HttpPost]
        public ActionResult Add(string message, string PatientName, string PatientEmail, string RoomNumber, string CardDesignNumber)
        {
            string query = "Insert into GetWellSoonCards (message, PatientName, PatientEmail , RoomNumber, CardDesignNumber) values (@message, @PatientName, @PatientEmail, @RoomNumber, @CardDesignNumber)";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@message", message);
            sqlparams[1] = new SqlParameter("@CardDesignNumber", CardDesignNumber);
            sqlparams[2] = new SqlParameter("@PatientName", PatientName);
            sqlparams[3] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[4] = new SqlParameter("@RoomNumber", RoomNumber);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            Debug.WriteLine("I am tryting to add the card with the message " + message);
            return RedirectToAction("List");
        }
        public ActionResult Show(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card id" + id);
            return View(Card);
        }
        public ActionResult Update(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card id" + id);
            return View(Card);
        }
        [HttpPost]
        public ActionResult Update(string Message, string PatientName, string PatientEmail, string RoomNumber, string CardDesignNumber, int CardId)
        {
            string query = "Update GetWellSoonCards set Message=@Message, PatientName=@PatientName, PatientEmail=@PatientEmail, RoomNumber=@RoomNumber, CardDesignNumber=@CardDesignNumber where CardId=@CardId";
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@Message", Message);
            sqlparams[1] = new SqlParameter("@CardDesignNumber", CardDesignNumber);
            sqlparams[2] = new SqlParameter("@PatientName", PatientName);
            sqlparams[3] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[4] = new SqlParameter("@RoomNumber", RoomNumber);
            sqlparams[5] = new SqlParameter("@CardId", CardId);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            Debug.WriteLine("I am tryting to edit the card with the message " + Message);
            return RedirectToAction("/Show/" + CardId);
        }
        public ActionResult Delete(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to delete card id" + id);
            return View(Card);
        }
        [HttpPost]
        public ActionResult Delete(int dsaid, int CardId)
        {
            string query = "Delete from GetWellSoonCards where CardId = @CardID";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CardId", CardId);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

    }
}