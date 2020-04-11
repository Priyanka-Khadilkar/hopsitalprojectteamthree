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
            eventModel.EventStartDate = DateTime.ParseExact(EventStartDate, "MM/dd/yyyy", null);
            eventModel.EventEndDate = DateTime.ParseExact(EventEndDate, "MM/dd/yyyy", null);
            eventModel.EventFromTime = EventFromTime;
            eventModel.EventToTime = EventToTime;
            eventModel.EventTargetAudience = EventTargetAudience;

            string currentUserId = User.Identity.GetUserId();
            eventModel.EventHostedBy = EventHostedBy;
            eventModel.EventDetails = EventDetail;
            eventModel.EventCreatedOn = DateTime.Now;

            eventModel.EventLocation = EventLocation;

            //Save Images
            string fileName = Guid.NewGuid() + Path.GetFileName(EventImage.FileName);
            string path = Path.Combine(Server.MapPath("~/Images/Event"),
                                       fileName);
            EventImage.SaveAs(path);
            eventModel.EventImagePath = fileName;

            //Get the current logged in user
            eventModel.EventCreater = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            eventModel.EventCreatedBy = currentUserId;
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

        /// <summary>
        /// Update the event details
        /// </summary>
        /// <returns>returns the update event</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            Event eventdata = db.Events.Include("EventCreater").Where(x => x.EventId == id).FirstOrDefault();
            //Return event detail
            return View(eventdata);
        }


        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, string EventTitle, string EventStartDate, string EventEndDate, string EventFromTime, string EventToTime, HttpPostedFileBase EventImage, string EventTargetAudience,
            string EventHostedBy, string EventDetail, string EventLocation)
        {
            Event eventdata = db.Events.Include("EventCreater").Where(x => x.EventId == id).FirstOrDefault();
            eventdata.EventTitle = EventTitle;
            eventdata.EventStartDate = DateTime.ParseExact(EventStartDate, "MM/dd/yyyy", null);
            eventdata.EventEndDate = DateTime.ParseExact(EventEndDate, "MM/dd/yyyy", null);
            eventdata.EventFromTime = EventFromTime;
            eventdata.EventToTime = EventToTime;
            eventdata.EventTargetAudience = EventTargetAudience;

            string currentUserId = User.Identity.GetUserId();
            eventdata.EventHostedBy = EventHostedBy;
            eventdata.EventDetails = EventDetail;
            eventdata.EventUpdatedOn = DateTime.Now;
            eventdata.EventUpdatedBy = currentUserId;
            eventdata.EventLocation = EventLocation;
            eventdata.EventUpdater = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (EventImage != null)
            {
                // Delete exiting file
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Images/Event"), eventdata.EventImagePath));
                // Save new file
                string fileName = Guid.NewGuid() + Path.GetFileName(EventImage.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/Event"), fileName);
                EventImage.SaveAs(path);
                eventdata.EventImagePath = fileName;
            }
            //Return event detail
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult Register(int id)
        {
            return RedirectToAction("RegistrationList");
        }

    }
}