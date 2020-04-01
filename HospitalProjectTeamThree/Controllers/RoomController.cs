using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Data;

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
            return View(rooms);
        }
    }
}