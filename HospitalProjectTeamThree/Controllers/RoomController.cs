using System;
using System.Collections.Generic;
using System.Data;

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
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace HospitalProjectTeamThree.Controllers
{
    public class RoomController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private RoomController() { }
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
        public ActionResult AdminView()
        {
            return View();
        }

      
        public ActionResult ShowOne(int? id)
        {


            //get all info about one room given the id
            Room Room = db.Rooms.SqlQuery("select * from Rooms where RoomID=@RoomID", new SqlParameter("@RoomID", id)).FirstOrDefault();


            //list all Rooms in the system
            string query = "Select * from Rooms ";
            List<Room> rooms = db.Rooms.SqlQuery(query).ToList();

            //trying to list one specific user by known UserName from AspNetUsers table

            //get the current user id when they logged in.  Code reference Paul Tran
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            //Debug.WriteLine("I want to see if I can get username from database" +User);


            //Adding data to viewmodel    
            FeaturedRoom viewmodel = new FeaturedRoom();
            viewmodel.rooms = rooms;
            viewmodel.room = Room;



            return View(viewmodel);
        }
       
    }
}