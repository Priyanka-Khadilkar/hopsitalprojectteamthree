using HospitalProjectTeamThree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Add(string EventTitle,string EventStartDate, string EventEndDate,string EventFromTime,string EventToTime,HttpPostedFileBase EventImage,string EventTargetAudience,
            string EventHostedBy, string EventDetail)
        {
            Event eventModel = new Event();
            eventModel.EventTitle = EventTitle;
            eventModel.EventStartDate = Convert.ToDateTime(EventStartDate);

            return View();
        }
    }
}