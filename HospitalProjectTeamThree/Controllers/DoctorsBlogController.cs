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
//using System.IO;

namespace HospitalProjectTeamThree.Controllers
{
    public class DoctorsBlogController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public ActionResult List()
        {
            string query = "Select * from DoctorsBlogs";
            List<DoctorsBlog> blogs = db.DoctorsBlogs.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the blogs");
            return View(blogs);
        }
        // GET details about one blog entry
        /*
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EF 6 technique
            DoctorsBlog doctorsblog = db.DoctorsBlogs.SqlQuery("select * from DoctorsBlogs where BlogId=@BlogId", new SqlParameter("@BlogId", id)).FirstOrDefault();
            if (doctorsblog == null)
            {
                return HttpNotFound();
            }

            

            string guest_query = "select * from Guests inner join TourBookingGuests on Guests.GuestID = TourBookingGuests.Guest_GuestID where TourBookingGuests.TourBooking_BookingID=@BookingID";
            var g_parameter = new SqlParameter("@BookingID", id);
            List<Guest> knownguests = db.Guests.SqlQuery(guest_query, g_parameter).ToList();

            string all_guests_query = "select * from Guests";
            List<Guest> AllGuests = db.Guests.SqlQuery(all_guests_query).ToList();

            // We use the AddParkGuest viewmodel so that we can show the guests that are on that booking and also so that we can see the dropdown list of guests
            // and add a guest to a booking if we want to
            AddParkGuest viewmodel = new AddParkGuest();
            viewmodel.Booking = tourbooking;
            viewmodel.Guests = knownguests;
            viewmodel.Add_Guest = AllGuests;

            return View(viewmodel);

        }*/
        public ActionResult Add()
        {
            // On the new blog entry we can select a topic
            List<BlogTopic> Topics = db.Topics.SqlQuery("select * from BlogTopics").ToList();

            //We use the AddParkGuest viewmodel to show the guessts and parks and be able to add them to the booking.
            AddBlogTopic AddBlogTopicViewModel = new AddBlogTopic();
            AddBlogTopicViewModel.BlogTopics = Topics;

            return View(AddBlogTopicViewModel);

        }
        [HttpPost]
        public ActionResult Add(string BlogTitle, string BlogContent, string BlogSource)
        {
            //Debug.WriteLine("Want to create a blog entry with a title of " + BlogTitle ) ;
            DateTime BlogDate = DateTime.Now;
            // We create the query to insert the values we will get into the database
            string query = "insert into DoctorsBlogs (BlogTitle, BlogContent, BlogSource, BlogDate) values (@BlogTitle,@BlogContent,@BlogSource, @BlogDate)";
            SqlParameter[] sqlparams = new SqlParameter[4];

            sqlparams[0] = new SqlParameter("@BlogTitle", BlogTitle);
            sqlparams[1] = new SqlParameter("@BlogContent", BlogContent);
            sqlparams[2] = new SqlParameter("@BlogSource", BlogSource);
            sqlparams[3] = new SqlParameter("@BlogDate", BlogDate);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            // Once added we go back to the list of blogs
            return RedirectToAction("List");
        }

    }
}