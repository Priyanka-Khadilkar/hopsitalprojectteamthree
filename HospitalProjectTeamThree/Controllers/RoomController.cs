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

namespace HospitalProjectTeamThree.Controllers
{
    public class RoomController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Room
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowAll()
        {

            string query = "Select * from Rooms ";
            List<Room> rooms = db.Rooms.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(rooms);
        }
    }
}