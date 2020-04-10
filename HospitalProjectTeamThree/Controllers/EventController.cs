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
    }
}