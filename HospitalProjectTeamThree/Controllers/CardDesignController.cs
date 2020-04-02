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
using HospitalProjectTeamThree.Models.ViewModels;
using System.Diagnostics;
using System.IO;
//using HospitalProjectTeamThree.Migrations;

namespace HospitalProjectTeamThree.Controllers
{
    public class CardDesignController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: CardDesign
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            string query = "Select * from CardDesigns";
            List<CardDesign> carddesigns = db.CardDesigns.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the cards");
            return View(carddesigns);
        }
    }
}