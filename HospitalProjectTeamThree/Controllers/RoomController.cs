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
        //private ApplicationSignInManager _signInManager;
        //private ApplicationUserManager _userManager;
        //private ApplicationRoleManager _roleManager;

        //private RoomController() { }
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Room
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShowAll()
        {
            //Shows list of all available rooms

            string query = "Select * from Rooms ";
            List<Room> rooms = db.Rooms.SqlQuery(query).ToList();
            //Debug.WriteLine("Checking connection to database");
            return View(rooms);
        }
      
        public ActionResult ShowOne(int? id)
        {
            //This displays one room selected by user, and other rooms available
            //get all info about one room given the id
            Room Room = db.Rooms.SqlQuery("select * from Rooms where RoomID=@RoomID", new SqlParameter("@RoomID", id)).FirstOrDefault();


            //list all Rooms in the system
            string query = "Select * from Rooms ";
            List<Room> rooms = db.Rooms.SqlQuery(query).ToList();

            //trying to list one specific user by known UserName from AspNetUsers table

            //get the current user id when they logged in.  Code reference Paul Tran
            //string userId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            //Debug.WriteLine("I want to see if I can get username from database" +UserId);


            //Adding data to viewmodel    
            FeaturedRoom viewmodel = new FeaturedRoom();
            viewmodel.rooms = rooms;
            viewmodel.room = Room;
           
            return View(viewmodel);
        }
        public ActionResult RoomBooking(int? id)
        {
            //shos room booked and information about logged in user

            //Debug.WriteLine("Room Id is: " +id);

            //get all info about one room given the id
            Room Rooms = db.Rooms.SqlQuery("select * from Rooms where RoomID=@RoomID", new SqlParameter("@RoomID", id)).FirstOrDefault();

            
            //get the current user id when they logged in. Code reference Paul Tran
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            
            //checking is  I get logged in user
            //Debug.WriteLine("User id is" + currentUser);
                        
            RoomBookings viewmodel = new RoomBookings();
            viewmodel.User = currentUser;
            viewmodel.Room = Rooms;
            return View(viewmodel);
        }
        //[HttpPost]
        public ActionResult Confirm(int? Id,string userid, string datefrom, string dateto, int roomId, bool payment)
        {
            //On this page user makes a booking and all information goes into the database

            // I used GET in form to generate the URL , now I know for sure what is passed 
            // sample URL:localhost:44325/Room/Confirm/499f6a93-9072-47f9-af2e-898178d3c14b?fname=Ivan&lname=Bob&email=ivan%40bob.com&email=ADp9uemS4jRrtoFlyY6Slq0PBD75eJjLudcIuOv5Q63z%2F5g%2F1xa9gd3bBw3%2BwzQR5g%3D%3D&roomselected=Semi-private
            //&datefrom=2020-05-05&dateto=2020-05-10&roomId=2&payment=1
            
            //Creating new record of a booking with information provided
            string query = "insert into RoomBookings (RoomID, UserId, PaymentCleared, DateFrom, DateTo) values (@roomId, @userid, @payment, @datefrom, @dateto)";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@roomId", roomId);
            sqlparams[1] = new SqlParameter("@userid", userid);
            sqlparams[2] = new SqlParameter("@payment", payment);
            sqlparams[3] = new SqlParameter("@datefrom", datefrom);
            sqlparams[4] = new SqlParameter("@dateto", dateto);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //get all info about one room given the id
            Room Rooms = db.Rooms.SqlQuery("select * from Rooms where RoomID=@roomId", new SqlParameter("@roomId", roomId)).FirstOrDefault();
            
            //get the current user id when they logged in. Code reference Paul Tran
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            //checking if  I get logged in user
            //Debug.WriteLine("User id is" + currentUser);
            
            //Getting information about specific booking given the user information
            
            RoomBooking selectedbooking = db.RoomBookings.SqlQuery("Select * from RoomBookings where UserId=@UserId", new SqlParameter("@UserId", userid)).FirstOrDefault();
                      
            //Assigning values to the viewmodel
            RoomBookings viewmodel = new RoomBookings();
            viewmodel.User = currentUser;
            viewmodel.Room = Rooms;
            viewmodel.AllRoomBookings = selectedbooking;
            return View(viewmodel);

        }
        public ActionResult AdminView()
        {
            return View();
        }
    }
}