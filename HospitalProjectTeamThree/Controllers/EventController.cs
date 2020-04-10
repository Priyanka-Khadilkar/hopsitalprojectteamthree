using HospitalProjectTeamThree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HospitalProjectTeamThree.Controllers
{
    public class EventController : Controller
    {
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
            string EventHostedBy, string EventDetail)
        {
            Event eventModel = new Event();
            eventModel.EventTitle = EventTitle;
            eventModel.EventStartDate = Convert.ToDateTime(EventStartDate);
            eventModel.EventEndDate = Convert.ToDateTime(EventEndDate);
            eventModel.EventTime = EventFromTime + " TO " + EventToTime;
            eventModel.EventTargetAudience = EventTargetAudience;

            string currentUserId = User.Identity.GetUserId();
            eventModel.EventHostedBy = currentUserId;
            eventModel.EventDetails = EventDetail;

            return View();
        }
    }
}