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
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Web.Mvc.Html;

namespace HospitalProjectTeamThree.Controllers
{
    public class LiveWaitTimeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();

        public LiveWaitTimeController() { }

        public ActionResult Index()
        {
            // Depending on the user that logs in they will be sent to the Live Wait Times department list,
            // Admin will be sent to the list
            // Editor will be sent to the public list
            // Registered Users will be sent to the public list with no access to delete, update or add
            // Guests will also be redirected to the public list
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("PublicList");
                }

            }
            else
            {
                return RedirectToAction("PublicList");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List(string deptsearchkey, int pagenum = 0)
        {
            List<Department> depts = db
                .Departments
                .Where(d => (deptsearchkey != null) ? d.DepartmentName.Contains(deptsearchkey) : true)
                .ToList();

            //we start the pagination, we include the searchkey in case there are more than 5 blogs that contain that word in the title
            int dperpage = 5;
            int deptcount = depts.Count();
            int maxpage = (int)Math.Ceiling((decimal)deptcount / dperpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(dperpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                depts = db.Departments
                    .Where(d => (deptsearchkey != null) ? d.DepartmentName.Contains(deptsearchkey) : true)
                    .OrderBy(d => d.DepartmentId)
                    .Skip(start)
                    .Take(dperpage)
                    .ToList();
            }
            else
            {
                // if there are less than the limit per page show page 1 of 1
                ViewData["pagesummary"] = "1 of 1";
            }

            //Debug.WriteLine("Iam trying to list all the departments");
            return View(depts);
        }
        public ActionResult PublicList(string deptsearchkey, int pagenum = 0)
        {
            List<Department> depts = db
                .Departments
                .Where(d => (deptsearchkey != null) ? d.DepartmentName.Contains(deptsearchkey) : true)
                .ToList();

            //we start the pagination, we include the searchkey in case there are more than 5 blogs that contain that word in the title
            int dperpage = 5;
            int deptcount = depts.Count();
            int maxpage = (int)Math.Ceiling((decimal)deptcount / dperpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(dperpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + " of " + (maxpage + 1);
                depts = db.Departments
                    .Where(d => (deptsearchkey != null) ? d.DepartmentName.Contains(deptsearchkey) : true)
                    .OrderBy(d => d.DepartmentId)
                    .Skip(start)
                    .Take(dperpage)
                    .ToList();
            }
            else
            {
                // if there are less than the limit per page show page 1 of 1
                ViewData["pagesummary"] = "1 of 1";
            }

            //Debug.WriteLine("Iam trying to list all the departments");
            return View(depts);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department SelectedDepartment = db.Departments.Find(id);

            List<LiveWaitTime> WaitTimes = db.LiveWaitTimes
                .Where(waittimes => waittimes.DepartmentId == id)
                .ToList();


            //Debug.WriteLine("Iam trying to list all the wait times updates");
            ShowLiveWaitTimes viewmodel = new ShowLiveWaitTimes();
            viewmodel.Departments = SelectedDepartment;
            viewmodel.WaitTimes = WaitTimes;

            return View(viewmodel);

        }
        public ActionResult PublicShow(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department SelectedDepartment = db.Departments.Find(id);

            List<LiveWaitTime> WaitTimes = db.LiveWaitTimes
                .Where(waittimes => waittimes.DepartmentId == id)
                .ToList();


            //Debug.WriteLine("Iam trying to list all the wait times updates");
            ShowLiveWaitTimes viewmodel = new ShowLiveWaitTimes();
            viewmodel.Departments = SelectedDepartment;
            viewmodel.WaitTimes = WaitTimes;

            return View(viewmodel);

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
           
            ShowLiveWaitTimes viewmodel = new ShowLiveWaitTimes();
            viewmodel.DepartmentsList = db.Departments.ToList();
            //we use the view model to be able to show the departments list when we add an update
            return View(viewmodel);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add(DateTime WaitUpdateDate, DateTime WaitUpdateTime, int DepartmentId, int WaitTimeList)
        {
            //Debug.WriteLine("Want to add an update with a department id of " + DepartmentId ) ;
            LiveWaitTime newUpdate = new LiveWaitTime();
            newUpdate.WaitUpdateDate = WaitUpdateDate;
            newUpdate.WaitUpdateTime = WaitUpdateTime;
            newUpdate.DepartmentId = DepartmentId;
            newUpdate.CurrentWaitTime = WaitTimeList;
            //we insert all the data into the database
            db.LiveWaitTimes.Add(newUpdate);
            db.SaveChanges();
            //once inserted we go back to the list
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            
            ShowLiveWaitTimes viewmodel = new ShowLiveWaitTimes();
            viewmodel.WaitTime = db.LiveWaitTimes.Find(id);
            viewmodel.DepartmentsList = db.Departments.ToList();
            //we use the viewmodel to get the information tu update

            return View(viewmodel);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Update(int id, DateTime WaitUpdateDate, DateTime WaitUpdateTime, int DepartmentId, int WaitTimeList)
        {
            //Debug.WriteLine("I want to edit a live wait time update time to " + WaitUpdateTime );
            LiveWaitTime SelectedUpdate = db.LiveWaitTimes.Find(id);
            SelectedUpdate.WaitUpdateDate = WaitUpdateDate;
            SelectedUpdate.WaitUpdateTime = WaitUpdateTime;
            SelectedUpdate.DepartmentId = DepartmentId;
            SelectedUpdate.CurrentWaitTime = WaitTimeList;
            //once updated we save to the database
            db.SaveChanges();
            //we redericet the admin to the list
            return RedirectToAction("List");

        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //Debug.WriteLine("update id is" + id);
            //we get the id of the update we want to delete from the database
            LiveWaitTime SelectedUpdate = db.LiveWaitTimes.Find(id);
            //we show it in our confirm page
            return View(SelectedUpdate);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteF(int id)
        {
            LiveWaitTime SelectedUpdate = db.LiveWaitTimes.Find(id);
            // once user confirms the delete the entry is removed from the database
            db.LiveWaitTimes.Remove(SelectedUpdate);

            db.SaveChanges();

            //and they are redirected to the list of departments
            return RedirectToAction("List");
        }
        public LiveWaitTimeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}