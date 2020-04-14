using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using System.Data.SqlClient;
using System.Net;
using System.Diagnostics;

namespace HospitalProjectTeamThree.Controllers
{
    public class MedicalStaffDirectoryController : Controller
    {
        // GET: MedicalStaffDirectory
       
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: Feedback

        [HttpPost]
        public ActionResult Add(string DepartmentId, string UserId)
        {

            string query = "insert into MedicalStaffDirectories (DepartmentId,UserId) values (@DepartmentId,@UserId)";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@DepartmentId", DepartmentId);
            //var UserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            sqlparams[1] = new SqlParameter("@UserId", UserId);
            db.Database.ExecuteSqlCommand(query, sqlparams);


            return RedirectToAction("List");
        }



        public ActionResult Add()
        {
            List<Department> Departments = db.Departments.SqlQuery("Select * from Departments").ToList();
            List<ApplicationUser> Users = db.Users.ToList();
         
            //Debug.WriteLine(users);
            AddUpdateMedicalStaffDirectory AddMedicalStaffDirectory = new AddUpdateMedicalStaffDirectory();
            AddMedicalStaffDirectory.Departments = Departments;
            AddMedicalStaffDirectory.Users = Users;
            return View(AddMedicalStaffDirectory);
        }

        //  [Authorize(Roles = "Admin,Editor")]
        public ActionResult List()

        {
            string currentUserId = User.Identity.GetUserId();
            if(String.IsNullOrEmpty(currentUserId)) { 
                return RedirectToAction("Login", "Account");
            } else
            {
                List<MedicalStaffDirectory> MedicalStaffDirectory = db.MedicalStaffDirectories.SqlQuery("Select * from MedicalStaffDirectories").ToList();
                return View(MedicalStaffDirectory);
            }
            
        }

        public ActionResult Update(int id)
        {
            MedicalStaffDirectory selectedmember = db.MedicalStaffDirectories.SqlQuery("Select * from MedicalStaffDirectories where MedicalStaffDirectoryId=@id", new SqlParameter("@id", id)).FirstOrDefault();
            List<Department> Departments = db.Departments.SqlQuery("Select * from Departments").ToList();
            List<ApplicationUser> Users = db.Users.ToList();
            AddUpdateMedicalStaffDirectory AddMedicalStaffDirectory = new AddUpdateMedicalStaffDirectory();
            
            AddMedicalStaffDirectory.MedicalStaffDirectory = selectedmember;
            AddMedicalStaffDirectory.Departments = Departments;
            AddMedicalStaffDirectory.Users = Users;
            return View(AddMedicalStaffDirectory);
        }

        //[HttpPost] 
        public ActionResult UpdateMedicalStaffDirectory(int medicalDirectoryId, int DepartmentId,string UserId)
        {
            Debug.WriteLine(UserId);
            string query = "Update MedicalStaffDirectories set DepartmentId=@DepartmentId,UserId=@UserId where MedicalStaffDirectoryId=@MedicalStaffDirectoryId";
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@DepartmentId", DepartmentId);
            //var UserId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            sqlparams[1] = new SqlParameter("@UserId", UserId);
            sqlparams[2] = new SqlParameter("@MedicalStaffDirectoryId", medicalDirectoryId);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        public ActionResult ConfirmDelete(int id)
        {
            string query = "delete from MedicalStaffDirectories where MedicalStaffDirectoryId=@id";
            SqlParameter sqlparams = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        // [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult Delete(int id)
        {
            string query = "select * from MedicalStaffDirectories where MedicalStaffDirectoryId = @id";
            SqlParameter sqlparams = new SqlParameter("@id", id);
            MedicalStaffDirectory selectedMember = db.MedicalStaffDirectories.SqlQuery(query, sqlparams).FirstOrDefault();

            return View(selectedMember);
        }
    }
}