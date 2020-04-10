using HospitalProjectTeamThree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Controllers
{
    public class EventController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Event
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(string EventTitle, string EventStartDate, string EventEndDate, string EventFromTime, string EventToTime, HttpPostedFileBase EventImage, string EventTargetAudience,
            string EventHostedBy, string EventDetail, string EventLocation)
        {
            Event eventModel = new Event();
            eventModel.EventTitle = EventTitle;
            eventModel.EventStartDate = Convert.ToDateTime(EventStartDate);
            eventModel.EventEndDate = Convert.ToDateTime(EventEndDate);
            eventModel.EventTime = EventFromTime + " TO " + EventToTime;
            eventModel.EventTargetAudience = EventTargetAudience;

            string currentUserId = User.Identity.GetUserId();
            eventModel.EventHostedBy = EventHostedBy;
            eventModel.EventDetails = EventDetail;
            eventModel.EventCreatedOn = DateTime.Now;
            eventModel.EventLocation = EventLocation;

            //Save Images
            string path = Path.Combine(Server.MapPath("~/Images/Event"),
                                       Path.GetFileName(EventImage.FileName));
            EventImage.SaveAs(path);
            eventModel.EventImagePath = Path.GetFileName(EventImage.FileName);

            //Get the current logged in user
            eventModel.EventCreater = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            db.Events.Add(eventModel);

            //Save data into database
            db.SaveChanges();
            return RedirectToAction("List");
        }

        /// <summary>
        /// List all Events
        /// </summary>
        /// <returns>returns list of Events </returns>
        public ActionResult List()
        {
            List<Event> events = new List<Event>();
            events = db.Events.Include("EventCreater").ToList();
            //Return all bookings to the listing page
            return View(events);
        }

        /// <summary>
        /// List all Events
        /// </summary>
        /// <returns>returns list of Events </returns>
        public ActionResult Show(int id)
        {
            Event eventdata = db.Events.Include("EventCreater").Where(x => x.EventId == id).FirstOrDefault();
            //Return event detail
            return View(eventdata);
        }

    }
}