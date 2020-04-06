using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using HospitalProjectTeamThree.Data;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security; 

namespace HospitalProjectTeamThree.Controllers
{
    public class JobListingController : Controller

    {
        //will be using managers to deal with login 
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public JobListingController() { }
        // GET: JobListing
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Editor"))
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Add");
                }
            }
            else
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult List(string jobsearchkey, int pagenum=0)
        {
            Debug.WriteLine("we are searching for " + jobsearchkey);

            string query = "Select * from JobListings";
            List<SqlParameter> sqlparams = new List<SqlParameter>();

            if(jobsearchkey!="")
            {
                query = query + "where JobTitle like @searchkey";
                sqlparams.Add(new SqlParameter("@searchkey", "%" + jobsearchkey + "%"));
                Debug.WriteLine("updated search should be looking for" + query);
            }

            List<JobListing> jobListings = db.JobListings.SqlQuery(query, sqlparams.ToArray()).ToList();

            //pagination
            int perpage = 5;
            int jobcount = jobListings.Count();
            int maxpage = (int)Math.Ceiling((decimal)jobcount / perpage) - 1;
            if (maxpage < 0) maxpage = 0;
            if (pagenum < 0) pagenum = 0;
            if (pagenum > maxpage) pagenum = maxpage;
            int start = (int)(perpage * pagenum);
            ViewData["pagenum"] = pagenum;
            ViewData["pagesummary"] = "";
            if (maxpage > 0)
            {
                ViewData["pagesummary"] = (pagenum + 1) + "of" + (maxpage + 1);
                List<SqlParameter> newparams = new List<SqlParameter>();

                if (jobsearchkey!="")
                {
                    newparams.Add(new SqlParameter("@searchkey", "%" + jobsearchkey + "%"));
                    ViewData["jobsearchkey"] = jobsearchkey;
                }
                newparams.Add(new SqlParameter("@start", start));
                newparams.Add(new SqlParameter("@perpage", perpage));
                string pagedquery = query + "order by JobID offset @start rows fetch first @perpage rows only";
                Debug.WriteLine(pagedquery);
                Debug.WriteLine("offset"+start);
                Debug.WriteLine("fetch first"+perpage);

                jobListings = db.JobListings.SqlQuery(pagedquery, newparams.ToArray()).ToList();
            }

            return View(jobListings);
        }

        public ActionResult Show(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobListing jobListing = db.JobListings.SqlQuery("select * from JobListings where JobID=@JobID", new SqlParameter("@JobID",id)).FirstOrDefault();
            if (jobListing == null)
            {
                return HttpNotFound();
            }

            return View(jobListing);
        }
        public ActionResult Add()
        {
            List<Department> departments = db.Departments.SqlQuery("select * from Departments").ToList();
            return View(departments);
        }

        [HttpPost]
        public ActionResult Add(string JobTitle, string JobDescription, Double Salary, DateTime StartDate, Boolean Published, int DepartmentID, int UserID)
        {
            string query = "insert into JobListings (JobTitle, JobDescription, Salary, StartDate, Published, DepartmentID, UserID)values (@JobTitle, @JobDescription, @Salary, @StartDate, @Published, @DepartmentID, @UserID)";
            SqlParameter[] sqlparams = new SqlParameter[7];

            sqlparams[0] = new SqlParameter("@JobTitle",JobTitle);
            sqlparams[1] = new SqlParameter("@JobDescription", JobDescription);
            sqlparams[2] = new SqlParameter("@Salary", Salary);
            sqlparams[3] = new SqlParameter("@StartDate",StartDate);
            sqlparams[4] = new SqlParameter("@Published",Published);
            sqlparams[5] = new SqlParameter("@DepartmentID",DepartmentID);
            sqlparams[6] = new SqlParameter("@UserID",UserID);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");

        }

        public ActionResult New()
        {
            List<JobListing> jobListings = db.JobListings.SqlQuery("select * from JobListings").ToList();
            return View(jobListings);

        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobListing jobListing = db.JobListings.SqlQuery("select * from JobListings where JobID=@JobID", new SqlParameter("@JobID", id)).FirstOrDefault();
          
            if (jobListing == null)
            {
                return HttpNotFound();
            }

            return View(jobListing);
        }
        
        [HttpPost]
        public ActionResult Update (int id, string JobTitle, string JobDescription, Double Salary, DateTime StartDate, Boolean Published, int DepartmentID, int UserID)
        {
            string query = "update JobListings set JobTitle=@JobTitle, JobDescription=@JobDescription, Salary=@Salary, StartDate=@StartDate, Published=@Published, DepartmentID=@DepartmentID, UserID=@UserID where JobID=@id";
            SqlParameter[] sqlparams = new SqlParameter[8];
            sqlparams[0] = new SqlParameter("@JobTitle", JobTitle);
            sqlparams[1] = new SqlParameter("@JobDescription", JobDescription);
            sqlparams[2] = new SqlParameter("@Salary", Salary);
            sqlparams[3] = new SqlParameter("@StartDate", StartDate);
            sqlparams[4] = new SqlParameter("@Published", Published);
            sqlparams[5] = new SqlParameter("@DepartmentID", DepartmentID);
            sqlparams[6] = new SqlParameter("@UserID", UserID);
            sqlparams[7] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("/Show/" + id);
        }
        public ActionResult DeleteConfirm (int id)
        {
            string query = "select * from JobListings where JobID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            JobListing selectedjob = db.JobListings.SqlQuery(query, param).FirstOrDefault();

            return View(selectedjob);
        }
        [Authorize(Roles="Admin, Registered User")]

        [HttpPost]
        public ActionResult Delete (int id)
        {
            string query = "delete from JobListings where JobID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

            return RedirectToAction("List");
        }
        public JobListingController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
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
    }
}