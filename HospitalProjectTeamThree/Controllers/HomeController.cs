using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalProjectTeamThree.Controllers
{
    public class HomeController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public ActionResult Index()
        {
            string query = "Select * from Crises ";
            List<Crisis> crises = db.Crisiss.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(crises);
           
        }
        //[Authorize(Roles = "Admin")]
        //Authorize let you set who have access to which page
        //For example, if you activate the above command, only admin can access the about page.
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}