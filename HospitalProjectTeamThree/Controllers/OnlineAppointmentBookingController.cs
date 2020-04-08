using HospitalProjectTeamThree.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HospitalProjectTeamThree.Controllers
{
    public class OnlineAppointmentBookingController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: OnlineAppointmentBooking

        public OnlineAppointmentBookingController()
        {

        }

        [Authorize(Roles = "Admin,NormalUser,Admin,Registered User")]
        public ActionResult Book()
        {
            //User.Identity.GetUserId() = this will get us loggedin user id
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(currentUser);
        }


        [Authorize(Roles = "Admin,NormalUser,Admin,Registered User")]
        [HttpPost]
        public ActionResult Book(string DateOfBirth, string PreferredDate, string PreferredTime, string PreferredDoctor)
        {
            //User.Identity.GetUserId() = this will get us loggedin user id
            //ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(currentUser);
        }


    }
}