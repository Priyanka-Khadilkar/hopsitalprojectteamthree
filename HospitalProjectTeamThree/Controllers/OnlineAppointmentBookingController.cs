using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace HospitalProjectTeamThree.Controllers
{
    //Developed by : Priyanka Khadilkar
    public class OnlineAppointmentBookingController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: OnlineAppointmentBooking

        /// <summary>
        /// load the appointment booking form - Only logged in user can book the appointment
        /// </summary>
        /// <returns>Returns the logged in user details</returns>
        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult Book()
        {
            //User.Identity.GetUserId() = this will get us loggedin user id
            string currentUserId = User.Identity.GetUserId();
            Debug.WriteLine("User id who is booking an appointment  : " + currentUserId);
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(currentUser);
        }


        /// <summary>
        /// Adding new online appointment booking record into database - Only logged in user can book appointments
        /// </summary>
        /// <param name="DateOfBirth"></param>
        /// <param name="PreferredDate"></param>
        /// <param name="PreferredTime"></param>
        /// <param name="PreferredDoctor"></param>
        /// <returns>returns to view of list of all appointments </returns>
        [Authorize(Roles = "Admin,Editor,Registered User")]
        [HttpPost]
        public ActionResult Book(string DateOfBirth, string PreferredDate, string PreferredTime, string PreferredDoctor)
        {
            //Set the appointment object to add in database
            OnlineAppointmentBooking booking = new OnlineAppointmentBooking();

            //Assign value to 
            booking.PatientDateOfBirth = DateTime.ParseExact(DateOfBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            booking.PreferredDate = DateTime.ParseExact(PreferredDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            booking.PreferredTime = PreferredTime;
            booking.PreferredDoctor = PreferredDoctor;
            booking.UserId = User.Identity.GetUserId();

            Debug.WriteLine("User id who is booking an appointment  : " + User.Identity.GetUserId());

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

        /// <summary>
        /// List all Online appointment
        /// </summary>
        /// <returns>returns list of appointments according to </returns>
        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult List(int? page, string filter, string SearchText)
        {
            List<OnlineAppointmentBooking> AllOnlineBookings = new List<OnlineAppointmentBooking>();
            //check if event has been searched or not
            if (SearchText != null)
            {
                //if it's searched so we have to set pagination
                page = 1;
            }
            else
            {
                //if it's not searched then maintain the filter into the list 
                filter = SearchText;
            }

            //Page size of the paging
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            ViewBag.filter = SearchText;
            Debug.WriteLine("Search key word to search into the list  : " + SearchText);

            string currentUserId = User.Identity.GetUserId();
            Debug.WriteLine("Current user id  : " + User.Identity.GetUserId());
            //If logged in user is Registered User
            if (User.IsInRole("Registered User"))
            {
                //registered user can only see booked appointment by themselves in past. I am still figuring out way to seaech data according to date.
                if (SearchText != null && SearchText != "")
                {
                    //When User search into list
                    AllOnlineBookings = db.OnlineAppointmentBookings.Include("User").Where(x => x.UserId == currentUserId && (x.PreferredTime.ToLower().Contains(SearchText.ToLower())
                    || x.PreferredDoctor.ToLower().Contains(SearchText.ToLower()))).ToList();
                }
                else
                {
                    // When User do not search into list
                    AllOnlineBookings = db.OnlineAppointmentBookings.Include("User").Where(x => x.UserId == currentUserId).ToList();
                }
            }
            //If logged in user is Admin or Editor
            else if (User.IsInRole("Admin") || User.IsInRole("Editor"))
            {
                //Admin and Editor user can see all the booked appointments and patient details
                if (SearchText != null && SearchText != "")
                {
                    // When User search into list
                    AllOnlineBookings = db.OnlineAppointmentBookings.Include("User").Where(x => x.User.FirstName.ToLower().Contains(SearchText.ToLower()) ||
                   x.User.LastName.ToLower().Contains(SearchText.ToLower()) || x.User.Email.ToLower().Contains(SearchText.ToLower()) || x.PreferredTime.ToLower().Contains(SearchText.ToLower())
                   || x.PreferredDoctor.ToLower().Contains(SearchText.ToLower())
                   ).ToList();

                }
                else
                {
                    // When User do not search into list
                    AllOnlineBookings = db.OnlineAppointmentBookings.Include("User").ToList();
                }
            }
            //Return all bookings to the listing page
            return View(AllOnlineBookings.ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Load booking details on form
        /// </summary>
        /// <param name="id">Online appointment booking id</param>
        /// <returns>Returns view model to update form</returns>
        [Authorize(Roles = "Admin,Editor")]
        public ActionResult Update(int id)
        {
            //ViewModel
            UpdateOnlineAppointmentBookingViewModel UpdateOnlineAppointmentBookingViewModel = new UpdateOnlineAppointmentBookingViewModel();

            Debug.WriteLine("Updating a booking appointment id  : " + id);
            //Get the appointment detail
            OnlineAppointmentBooking OnlineAppointmentBooking = db.OnlineAppointmentBookings.Include("User").Where(x => x.OnlineAppointmentBookingId == id).FirstOrDefault();

            //Assign value to viewmodel
            UpdateOnlineAppointmentBookingViewModel.OnlineAppointmentBooking = OnlineAppointmentBooking;
            UpdateOnlineAppointmentBookingViewModel.OnlineAppointmentBookingStatus = (OnlineAppointmentBookingStatus)Enum.ToObject(typeof(OnlineAppointmentBookingStatus), Convert.ToInt32(OnlineAppointmentBooking.OnlineAppointmentBookingStatus));

            //Return all bookings to the listing page
            return View(UpdateOnlineAppointmentBookingViewModel);
        }

        /// <summary>
        ///Admin and Editor can update the online booked appoinment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="DateOfBirth"></param>
        /// <param name="PreferredDate"></param>
        /// <param name="PreferredTime"></param>
        /// <param name="PreferredDoctor"></param>
        /// <param name="OnlineAppointmentBookingStatus"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Editor")]
        [HttpPost]
        public ActionResult Update(int id, string DateOfBirth, string PreferredDate, string PreferredTime, string PreferredDoctor, int OnlineAppointmentBookingStatus)
        {
            Debug.WriteLine("Updating a booking appointment id  : " + id);
            //Get the booked appoitment booking details
            OnlineAppointmentBooking onlineAppointmentBooking = db.OnlineAppointmentBookings.Include("User").Where(x => x.OnlineAppointmentBookingId == id).FirstOrDefault();

            //Assign the updated details 
            onlineAppointmentBooking.PatientDateOfBirth = Convert.ToDateTime(DateOfBirth);
            onlineAppointmentBooking.PreferredDate = Convert.ToDateTime(PreferredDate);
            onlineAppointmentBooking.PreferredTime = PreferredTime;
            onlineAppointmentBooking.PreferredDoctor = PreferredDoctor;
            onlineAppointmentBooking.OnlineAppointmentBookingStatus = OnlineAppointmentBookingStatus;

            //Update the booking details online
            db.SaveChanges();
            //Return all bookings to the listing page
            return RedirectToAction("List");
        }

        //only logged in user can view the details of the 
        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult View(int id)
        {
            Debug.WriteLine("viwe a booking appointment id  : " + id);
            //Get the booked appoitment booking details according to Id
            OnlineAppointmentBooking onlineAppointmentBooking = db.OnlineAppointmentBookings.Include("User").Where(x => x.OnlineAppointmentBookingId == id).FirstOrDefault();
            //Return booking detail
            return View(onlineAppointmentBooking);
        }


        /// <summary>
        /// Update the status of the booking to cancelled.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult UpdateStatus(int id)
        {
            //Update the status of the booking - logged in user can cancel the past booked appointment if the status is booked or cancelled. 
            OnlineAppointmentBooking onlineAppointmentBooking = db.OnlineAppointmentBookings.Include("User").Where(x => x.OnlineAppointmentBookingId == id).FirstOrDefault();
            onlineAppointmentBooking.OnlineAppointmentBookingStatus = (int)OnlineAppointmentBookingStatus.Cancelled;

            Debug.WriteLine("update booking status appointment id  : " + id);

            //save into database
            db.SaveChanges();
            return RedirectToAction("List");
        }

    }
}