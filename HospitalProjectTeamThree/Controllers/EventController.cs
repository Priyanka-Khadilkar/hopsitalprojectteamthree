using HospitalProjectTeamThree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using HospitalProjectTeamThree.Data;
using System.Data.SqlClient;
using HospitalProjectTeamThree.Models.ViewModels;
using System.Data.Entity;
using System.Globalization;
using System.Diagnostics;

namespace HospitalProjectTeamThree.Controllers
{
    public class EventController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();

        // Add event - Only Admin allow to add an event
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        //I am using CKEditor so get it worked i found this solution to insert into database, i know it's not safe i found AllowHtml attribute
        //however, currently i am figuring out how to user model for validation.
        [ValidateInput(false)]
        // Add event - Only Admin allow to add an event
        [Authorize(Roles = "Admin")]
        public ActionResult Add(string EventTitle, string EventStartDate, string EventEndDate, string EventFromTime, string EventToTime, HttpPostedFileBase EventImage, string EventTargetAudience,
            string EventHostedBy, string EventDetail, string EventLocation)
        {
            //Event Object for adding an event
            Event eventModel = new Event();

            //Set all value to the event object
            eventModel.EventTitle = EventTitle;
            eventModel.EventStartDate = DateTime.ParseExact(EventStartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            eventModel.EventEndDate = DateTime.ParseExact(EventEndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            eventModel.EventFromTime = EventFromTime;
            eventModel.EventToTime = EventToTime;
            eventModel.EventTargetAudience = EventTargetAudience;

            //Get logged in User Id
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

            events = db.Events.Include(x => x.EventCreater).ToList();
            //Return all events to the listing page
            return View(events);
        }

        /// <summary>
        /// List all Events
        /// </summary>
        /// <returns>returns list of Events </returns>
        public ActionResult Show(int id)
        {
            Event eventdata = db.Events.Include(x => x.EventCreater).Where(x => x.EventId == id).FirstOrDefault();
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
            Event eventdata = db.Events.Include(x => x.EventCreater).Where(x => x.EventId == id).FirstOrDefault();
            //Return event detail
            return View(eventdata);
        }


        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, string EventTitle, string EventStartDate, string EventEndDate, string EventFromTime, string EventToTime, HttpPostedFileBase EventImage, string EventTargetAudience,
            string EventHostedBy, string EventDetail, string EventLocation)
        {
            Event eventdata = db.Events.Include(x => x.EventCreater).Where(x => x.EventId == id).FirstOrDefault();
            eventdata.EventTitle = EventTitle;
            eventdata.EventStartDate = DateTime.ParseExact(EventStartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            eventdata.EventEndDate = DateTime.ParseExact(EventEndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
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
            //Get the current logged in user id.
            string currentUserId = User.Identity.GetUserId();

            //Query to insert data into bridging table of event and user.register user for events
            //string query = "Insert into AspNetUserEvents (UserId,EventId) values (@UserId, @EventId)";
            //SqlParameter[] sqlparams = new SqlParameter[2];
            //sqlparams[0] = new SqlParameter("@UserId", currentUserId);
            //sqlparams[1] = new SqlParameter("@EventId", id);
            ////Execute
            //db.Database.ExecuteSqlCommand(query, sqlparams);

            //Find the logged in user
            ApplicationUser LoggedInUser = db.Users.Find(currentUserId);
            //Check the user data is not null
            if (LoggedInUser != null)
            {
                //Check if loggedin User has already registered for the event or not 
                Event CheckEventAlreadyExistOrNot = LoggedInUser.Events.Where(x => x.EventId == id).FirstOrDefault();
                if (CheckEventAlreadyExistOrNot == null)
                {
                    //if  logged in user has not already registred then allow to register into system.
                    Event selectedEvent = db.Events.Find(id);
                    LoggedInUser.Events.Add(selectedEvent);
                    db.SaveChanges();
                }

            }

            //Return list of the registration List of events
            return RedirectToAction("RegistrationList");
        }

        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult RegistrationList()
        {
            //Get the current logged in user id.
            string currentUserId = User.Identity.GetUserId();
            //inner join with user table to get all registrerd events in past.
            List<Event> RegistrationList = db.Users.Where(X => X.Id == currentUserId).SelectMany(c => c.Events).ToList();
            return View(RegistrationList);
        }

        //Only Admin can access the data of registered users
        [Authorize(Roles = "Admin")]
        public ActionResult RegisteredUserList(int id)
        {
            RegisteredUserList registeredUserList = new RegisteredUserList();

            //Get All registrerd users list with  selected event
            List<ApplicationUser> RegisteredUserList = db.Events.Where(x => x.EventId == id).SelectMany(c => c.Users).ToList();

            //Select Event details
            Event eventData = db.Events.Where(x => x.EventId == id).FirstOrDefault();
            registeredUserList.EventData = eventData;
            registeredUserList.UserList = RegisteredUserList;
            return View(registeredUserList);

        }

        //Only Admin can access delete the event
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("Event to Delete  : " + id);
            string RegisteredUserDeleteQuery = "delete from AspNetUserEvents where EventId = @id";
            SqlParameter RegisteredUserParam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(RegisteredUserDeleteQuery, RegisteredUserParam);

            // Delete Event
            string DeleteEvent = "delete from Events where EventId = @id";
            SqlParameter DeleteEventParam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(DeleteEvent, DeleteEventParam);
            return RedirectToAction("List");
        }

    }
}