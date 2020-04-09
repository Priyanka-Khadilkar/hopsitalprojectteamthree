using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// load the appointment booking form
        /// </summary>
        /// <returns>Returns the logged in user details</returns>
        [Authorize(Roles = "Admin,NormalUser,Editor,Registered User")]
        public ActionResult Book()
        {
            //User.Identity.GetUserId() = this will get us loggedin user id
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(currentUser);
        }


        /// <summary>
        /// Adding new online appointment booking record into database
        /// </summary>
        /// <param name="DateOfBirth"></param>
        /// <param name="PreferredDate"></param>
        /// <param name="PreferredTime"></param>
        /// <param name="PreferredDoctor"></param>
        /// <returns>returns to view of list of all appointments </returns>
        [Authorize(Roles = "Admin,NormalUser,Editor,Registered User")]
        [HttpPost]
        public ActionResult Book(string DateOfBirth, string PreferredDate, string PreferredTime, string PreferredDoctor)
        {
            //Set the appointment object to add in database
            OnlineAppointmentBooking booking = new OnlineAppointmentBooking();
            booking.PatientDateOfBirth = Convert.ToDateTime(DateOfBirth);
            booking.PreferredDate = Convert.ToDateTime(PreferredDate);
            booking.PreferredTime = PreferredTime;
            booking.PreferredDoctor = PreferredDoctor;
            booking.UserId = User.Identity.GetUserId();
            booking.OnlineAppointmentBookingBookedOn = DateTime.Now;
            booking.OnlineAppointmentBookingStatus = (int)OnlineAppointmentBookingStatus.InProcess;
            string currentUserId = User.Identity.GetUserId();

            //Get current user to set the FK data for an appointment.
            booking.User = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            //Add appointment record in to the databse
            db.OnlineAppointmentBookings.Add(booking);

            //Save data into database
            db.SaveChanges();
            //Redirect to List Page
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin,NormalUser,Editor,Registered User")]
        public ActionResult List()
        {
            List<OnlineAppointmentBooking> AllOnlineBookings = new List<OnlineAppointmentBooking>();
            string currentUserId = User.Identity.GetUserId();
            //If logged in user is Registered User
            if (User.IsInRole("Registered User"))
            {
                //registered user can only see booked appointment by themselves in past.
                AllOnlineBookings = db.OnlineAppointmentBookings.Include("User").Where(x => x.UserId == currentUserId).ToList();
            }
            //If logged in user is Admin or Editor
            else if (User.IsInRole("Admin") || User.IsInRole("Editor"))
            {
                //Admin and Editor user can see all the booked appointments and patient details
                AllOnlineBookings = db.OnlineAppointmentBookings.Include("User").ToList();
            }
            //Return all bookings to the listing page
            return View(AllOnlineBookings);
        }

        [Authorize(Roles = "Admin,NormalUser,Editor,Registered User")]
        public ActionResult Update(int id)
        {
            UpdateOnlineAppointmentBookingViewModel UpdateOnlineAppointmentBookingViewModel = new UpdateOnlineAppointmentBookingViewModel();
            OnlineAppointmentBooking OnlineAppointmentBooking = db.OnlineAppointmentBookings.Include("User").Where(x => x.OnlineAppointmentBookingId == id).FirstOrDefault();
            UpdateOnlineAppointmentBookingViewModel.OnlineAppointmentBooking = OnlineAppointmentBooking;
            UpdateOnlineAppointmentBookingViewModel.OnlineAppointmentBookingStatus = (OnlineAppointmentBookingStatus)Enum.ToObject(typeof(OnlineAppointmentBookingStatus), Convert.ToInt32(OnlineAppointmentBooking.OnlineAppointmentBookingStatus));
            //Return all bookings to the listing page
            return View(UpdateOnlineAppointmentBookingViewModel);
        }

        //[HttpPost]
        //public ActionResult Update(int id,string DateOfBirth,string PreferredDate,string PreferredTime, string PreferredDoctor,string OnlineAppointmentBookingStatus)
        //{


        //    //Return all bookings to the listing page
        //    return View(AllOnlineBookings);
        //}


        public ActionResult UpdateStatus(int id)
        {
            OnlineAppointmentBooking onlineAppointmentBooking = db.OnlineAppointmentBookings.Include("User").Where(x => x.OnlineAppointmentBookingId == id).FirstOrDefault();
            onlineAppointmentBooking.OnlineAppointmentBookingStatus = (int)OnlineAppointmentBookingStatus.Cancelled;
            db.SaveChanges();
            return RedirectToAction("List");
        }


    }
}