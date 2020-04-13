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
using PagedList;

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
        //however, currently i am figuring out how to use model for validation.
        [ValidateInput(false)]
        // Add event - Only Admin allow to add an event
        [Authorize(Roles = "Admin")]
        public ActionResult Add(string EventTitle, string EventStartDate, string EventEndDate, string EventFromTime, string EventToTime, HttpPostedFileBase EventImage, string EventTargetAudience,
            string EventHostedBy, string EventDetail, string EventLocation)
        {

            //Future plan is to validate all fields client side with model implementation and then insert into database

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
            Debug.WriteLine("current user Id  : " + currentUserId);
            eventModel.EventHostedBy = EventHostedBy;
            eventModel.EventDetails = EventDetail;
            eventModel.EventCreatedOn = DateTime.Now;

            eventModel.EventLocation = EventLocation;

            //Check the image is posted 
            if (EventImage != null)
            {
                //Save Images with the unique name
                string fileName = Guid.NewGuid() + Path.GetFileName(EventImage.FileName);
                Debug.WriteLine("Event file name  : " + fileName);
                string path = Path.Combine(Server.MapPath("~/Images/Event"),
                                           fileName);
                EventImage.SaveAs(path);
                eventModel.EventImagePath = fileName;
            }
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
        public ActionResult List(int? page, string filter, string SearchText)
        {
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

            Debug.WriteLine("current page number is  : " + page);
            //Page size of the paging
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            ViewBag.filter = SearchText;
            Debug.WriteLine("Search text to search into list is : " + SearchText);
            if (SearchText != null && SearchText != "")
            {
                //filter the list according to search text. I am still figuring out to filter data according to date
                List<Event> events = db.Events.Where(e => e.EventTitle.ToLower().Contains(SearchText.ToLower()) ||
                e.EventLocation.ToLower().Contains(SearchText.ToLower()) || e.EventTargetAudience.ToLower().Contains(SearchText.ToLower()) ||
                e.EventHostedBy.ToLower().Contains(SearchText.ToLower()) || e.EventFromTime.ToLower().Contains(SearchText.ToLower()) || e.EventToTime.ToLower().Contains(SearchText.ToLower())
                ).ToList();

                //Convert to paged list
                return View(events.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                //List events 
                List<Event> events = db.Events.ToList();
                return View(events.ToPagedList(pageNumber, pageSize));
            }

        }

        /// <summary>
        /// List all Events
        /// </summary>
        /// <returns>returns list of Events </returns>
        public ActionResult Show(int id)
        {
            Debug.WriteLine("Event Id to show : " + id);
            Event eventdata = db.Events.Include(x => x.EventCreater).Where(x => x.EventId == id).FirstOrDefault();
            //Return event detail
            return View(eventdata);
        }

        /// <summary>
        /// Update the event details - Only Admin can update the event data
        /// </summary>
        /// <returns>returns the update event</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            Debug.WriteLine("Event Id to update : " + id);
            //Query to get event data according to Id
            Event eventdata = db.Events.Include(x => x.EventCreater).Where(x => x.EventId == id).FirstOrDefault();
            //Return event detail
            return View(eventdata);
        }


        //Only admin can update the event data
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, string EventTitle, string EventStartDate, string EventEndDate, string EventFromTime, string EventToTime, HttpPostedFileBase EventImage, string EventTargetAudience,
            string EventHostedBy, string EventDetail, string EventLocation)
        {
            //Fetch the event to update
            Event eventdata = db.Events.Include(x => x.EventCreater).Where(x => x.EventId == id).FirstOrDefault();

            //update the event data
            eventdata.EventTitle = EventTitle;
            eventdata.EventStartDate = DateTime.ParseExact(EventStartDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            eventdata.EventEndDate = DateTime.ParseExact(EventEndDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            eventdata.EventFromTime = EventFromTime;
            eventdata.EventToTime = EventToTime;
            eventdata.EventTargetAudience = EventTargetAudience;
            string currentUserId = User.Identity.GetUserId();
            Debug.WriteLine("Logged in user ud : " + currentUserId);
            eventdata.EventHostedBy = EventHostedBy;
            eventdata.EventDetails = EventDetail;
            eventdata.EventUpdatedOn = DateTime.Now;
            eventdata.EventUpdatedBy = currentUserId;
            eventdata.EventLocation = EventLocation;
            eventdata.EventUpdater = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            //Update the image data into database as well as delete the old image
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

            //update the data into database
            db.SaveChanges();

            //return to the list of event 
            return RedirectToAction("List");
        }

        //Only logged in user can register for the events
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
            Debug.WriteLine("Logged in user ud : " + currentUserId);
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

        //Loggedin user can see their past registered events data.
        [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult RegistrationList()
        {
            //Get the current logged in user id.
            string currentUserId = User.Identity.GetUserId();
            Debug.WriteLine("Logged in user ud : " + currentUserId);
            //inner join with user table to get all registrerd events in past.
            List<Event> RegistrationList = db.Users.Where(X => X.Id == currentUserId).SelectMany(c => c.Events).ToList();
            return View(RegistrationList);
        }

        //Only Admin can access the data of registered users
        [Authorize(Roles = "Admin")]
        public ActionResult RegisteredUserList(int id)
        {
            RegisteredUserList registeredUserList = new RegisteredUserList();

            Debug.WriteLine("Event id : " + id);

            //Get All registrerd users list with  selected event
            List<ApplicationUser> RegisteredUserList = db.Events.Where(x => x.EventId == id).SelectMany(c => c.Users).ToList();

            //Select Event details
            Event eventData = db.Events.Where(x => x.EventId == id).FirstOrDefault();
            registeredUserList.EventData = eventData;
            registeredUserList.UserList = RegisteredUserList;

            //return the list of registered user list
            return View(registeredUserList);

        }

        /// <summary>
        /// Delete the Event 
        /// </summary>
        /// <param name="id">Id of the event</param>
        /// <returns></returns>
        //Only Admin can access delete the event
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("Event to Delete  : " + id);

            //delete the event from the user event bridging table
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