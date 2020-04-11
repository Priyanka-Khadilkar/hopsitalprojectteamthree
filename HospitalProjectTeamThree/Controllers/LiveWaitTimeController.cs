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
//need this for pagination
using PagedList;
using Microsoft.AspNet.Identity;

namespace HospitalProjectTeamThree.Controllers
{
    public class LiveWaitTimeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();

        public LiveWaitTimeController() { }

        public ActionResult PublicList()
        {
            List<Department> Departments = db.Departments.ToList();

            //Debug.WriteLine("Iam trying to list all the departments");
            return View(Departments);
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


            //Debug.WriteLine("Iam trying to list all the blogs");
            ShowLiveWaitTimes viewmodel = new ShowLiveWaitTimes();
            viewmodel.Departments = SelectedDepartment;
            viewmodel.WaitTimes = WaitTimes;

            return View(viewmodel);

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