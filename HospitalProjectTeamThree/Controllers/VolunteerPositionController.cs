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
using PagedList;

namespace HospitalProjectTeamThree.Controllers
{
    public class VolunteerPositionController : Controller
    {
        //will be using managers to deal with login 
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public VolunteerPositionController() { }
        // GET: VolunteerPosition
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
        public ActionResult List (string volunteerpositionsearchkey, int? page)
        {
            Debug.WriteLine("we are searching for " + volunteerpositionsearchkey);

            List<VolunteerPosition> volunteerPositions = db.VolunteerPositions.ToList();
            
            //pagination
            int perpage = 5;
            int pagenum = (page ?? 1);


            return View(volunteerPositions.ToPagedList(pagenum, perpage));

            //search function
            //  if(jobsearchkey!="")
            // {
            //    query = query + "where JobTitle like @searchkey";
            //    sqlparams.Add(new SqlParameter("@searchkey", "%" + jobsearchkey + "%"));
            //  Debug.WriteLine("updated search should be looking for" + query);
            // }

            //  List<VolunteerPosition> volunteerPositions = db.VolunteerPositions.SqlQuery(query, sqlparams.ToArray()).ToList();


            // int positioncount = volunteerPositions.Count();
            //  int maxpage = (int)Math.Ceiling((decimal)positioncount / perpage) - 1;
            //  if (maxpage < 0) maxpage = 0;
            // if (pagenum < 0) pagenum = 0;
            // if (pagenum > maxpage) pagenum = maxpage;
            //  int start = (int)(perpage * pagenum);
            //  ViewData["pagenum"] = pagenum;
            //  ViewData["pagesummary"] = "";
            // if (maxpage > 0)
            // {
            //   ViewData["pagesummary"] = (pagenum + 1) + "of" + (maxpage + 1);
            //   List<SqlParameter> newparams = new List<SqlParameter>();

            // if (jobsearchkey!="")
            // {
            //   newparams.Add(new SqlParameter("@searchkey", "%" + jobsearchkey + "%"));
            // ViewData["jobsearchkey"] = jobsearchkey;
            // }
            //  newparams.Add(new SqlParameter("@start", start));
            //  newparams.Add(new SqlParameter("@perpage", perpage));
            //  string pagedquery = query +  " order by VolunteerPositionID offset @start rows fetch first @perpage rows only";
            //   Debug.WriteLine(pagedquery);
            //  Debug.WriteLine("offset" + start);
            //  Debug.WriteLine("fetch first" + perpage);

            //  volunteerPositions = db.VolunteerPositions.SqlQuery(pagedquery, newparams.ToArray()).ToList();
            //  }


        }
        [Authorize(Roles = "Admin, Editor" )]
        public ActionResult Add()
        {
            List<Department> Departments = db.Departments.SqlQuery("select * from departments").ToList();
            
            AddVolunteerPosition viewModel = new AddVolunteerPosition();
            viewModel.Departments = Departments;
           
            return View(viewModel);
        }
        [HttpPost]
     
        public ActionResult Add(string VolunteerPositionTitle, string VolunteerPositionDescription, DateTime StartDate, int DepartmentID)
        {
            string query = "insert into VolunteerPositions (VolunteerPositionTitle, VolunteerPositionDescription, StartDate, DepartmentID)values (@VolunteerPositionTitle, @VolunteerPositionDescription, @StartDate, @DepartmentID)";
            SqlParameter[] sqlparams = new SqlParameter[4];

            sqlparams[0] = new SqlParameter("@VolunteerPositionTitle", VolunteerPositionTitle);
            sqlparams[1] = new SqlParameter("@VolunteerPositionDescription", VolunteerPositionDescription);
            sqlparams[2] = new SqlParameter("@StartDate", StartDate);
            sqlparams[3] = new SqlParameter("@DepartmentID", DepartmentID);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
        public ActionResult Show(int? id)
        {
           VolunteerPosition volunteerPosition = db.VolunteerPositions.SqlQuery("select * from VolunteerPositions where VolunteerPositionID = @VolunteerPositionID", new SqlParameter("@VolunteerPositionID", id)).FirstOrDefault();
           //List <VolunteerPosition> volunteerPosition = db.VolunteerPositions.SqlQuery("select * from VolunteerPositions inner join VolunteerPositionApplicationUsers " +
             //    "on VolunteerPositionApplicationUsers.VolunteerPosition_VolunteerPositionID = VolunteerPositions.VolunteerPositionID " +
               //  "where VolunteerPosition_VolunteerPositionID = @VolunteerPositionID", new SqlParameter("@VolunteerPositionID", id)).ToList();
             

            List < Department> department = db.Departments.SqlQuery("select * from Departments inner join VolunteerPositions on VolunteerPositions.DepartmentID = Departments.DepartmentID where VolunteerPositionID = @id", new SqlParameter("@id", id)).ToList();
            List<ApplicationUser> volunteers = db.VolunteerPositions.Where(x => x.VolunteerPositionID == id).SelectMany(c => c.Users).ToList();


            // string query = "select * from VolunteerPositionApplicationUsers where VolunteerPosition_VolunteerPositionID = @VolunteerPositionID";
            //  var fk_param = new SqlParameter("@VolunteerPositionID", id);

            //SqlParameter[] sqlparams = new SqlParameter[1];
            // sqlparams[0] = new SqlParameter("@VolunteerPositionID", id);
            //  List<ApplicationUser> volunteers = db.VolunteerPositions.SqlQuery(query, sqlparams).ToList();

             // string fk_query = "select * from VolunteerPositions inner join VolunteerPositionApplicationUsers " +
               //  "on VolunteerPositionApplicationUsers.VolunteerPosition_VolunteerPositionID = VolunteerPositions.VolunteerPositionID " +
              //   "where VolunteerPosition_VolunteerPositionID = @VolunteerPositionID";
            //  var fk_param = new SqlParameter("@VolunteerPositionID", id);
             // List<VolunteerPosition> volunteerPosition = db.VolunteerPositions.SqlQuery(fk_query, fk_param).ToList();




            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (volunteerPosition == null)
            {
                return HttpNotFound();
            }
            ShowVolunteerPosition ShowVolunteerPositionViewModel = new ShowVolunteerPosition();
            ShowVolunteerPositionViewModel.VolunteerPosition = volunteerPosition;
            ShowVolunteerPositionViewModel.Departments = department;
            ShowVolunteerPositionViewModel.Users = volunteers;


            return View(ShowVolunteerPositionViewModel);

        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Update(int id)
        {
            VolunteerPosition selectedVolunteerPosition = db.VolunteerPositions.SqlQuery("select * from VolunteerPositions where VolunteerPositionID=@VolunteerPositionID", new SqlParameter("@VolunteerPositionID", id)).FirstOrDefault();
            List<Department> departments = db.Departments.SqlQuery("select * from Departments").ToList();
            List<ApplicationUser> volunteers = db.VolunteerPositions.Where(x => x.VolunteerPositionID == id).SelectMany(c => c.Users).ToList();
            // string userId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            UpdateVolunteerPosition UpdateVolunteerPositionViewModel = new UpdateVolunteerPosition();
            UpdateVolunteerPositionViewModel.VolunteerPosition = selectedVolunteerPosition;
            UpdateVolunteerPositionViewModel.Departments = departments;
            UpdateVolunteerPositionViewModel.Users = volunteers;
           // UpdateJobListingViewModel.User = currentUser;

            return View(UpdateVolunteerPositionViewModel);

        }
        [HttpPost]
        public ActionResult Update(int id, string VolunteerPositionTitle, string VolunteerPositionDescription, DateTime StartDate, int DepartmentID )
        {
            string query = "update VolunteerPositions set VolunteerPositionTitle=@VolunteerPositionTitle, VolunteerPositionDescription=@VolunteerPositionDescription, StartDate=@StartDate, DepartmentID=@DepartmentID where VolunteerPositionID=@id";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@VolunteerPositionTitle", VolunteerPositionTitle);
            sqlparams[1] = new SqlParameter("@VolunteerPositionDescription", VolunteerPositionDescription);
            sqlparams[2] = new SqlParameter("@StartDate", StartDate);
            sqlparams[3] = new SqlParameter("@DepartmentID", DepartmentID);
          //  sqlparams[6] = new SqlParameter("@UserID", UserID);
            sqlparams[4] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
        public ActionResult Delete(int id)
        {
            VolunteerPosition position = db.VolunteerPositions.SqlQuery("select * from VolunteerPositions where VolunteerPositionID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            List<Department> department = db.Departments.SqlQuery("select * from Departments inner join VolunteerPositions on VolunteerPositions.DepartmentID = Departments.DepartmentID where VolunteerPositionID = @id", new SqlParameter("@id", id)).ToList();

            ShowVolunteerPosition ShowVolunteerPositionViewModel = new ShowVolunteerPosition();
            ShowVolunteerPositionViewModel.VolunteerPosition = position;
            ShowVolunteerPositionViewModel.Departments = department;

            return View(ShowVolunteerPositionViewModel);
        }
        [HttpPost]
        public ActionResult Delete(int id, int VolunteerPositionID)
        {
            string query = "delete from VolunteerPositions where VolunteerPositionID = @VolunteerPositionID";
            SqlParameter param = new SqlParameter("@VolunteerPositionID", VolunteerPositionID);
            db.Database.ExecuteSqlCommand(query, param);

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult SignUp(string id, string UserID)
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            UserID = userId;

            //check if the user has already signed up for the volunteer position

            string check_query = "select * from VolunteerPositions inner join VolunteerPositionApplicationUsers " +
                "on VolunteerPositionApplicationUsers.VolunteerPosition_VolunteerPositionID = VolunteerPositions.VolunteerPositionID " +
                "where VolunteerPosition_VolunteerPositionID=@VolunteerPositionID and ApplicationUser_ID = @id";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@id", UserID);
            check_params[1] = new SqlParameter("@VolunteerPositionID", id);

            List<VolunteerPosition> positions = db.VolunteerPositions.SqlQuery(check_query, check_params).ToList();
            if(positions.Count <= 0)
            {
                string query = "insert into VolunteerPositionApplicationUsers (VolunteerPosition_VolunteerPositionID, ApplicationUser_Id)values(@VolunteerPositionID, @ApplicationUserID)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@VolunteerPositionID", id);
                sqlparams[1] = new SqlParameter("@ApplicationUserID", UserID);

                db.Database.ExecuteSqlCommand(query, sqlparams);


            }
            return RedirectToAction("Show/" + id);

        }

    }
}