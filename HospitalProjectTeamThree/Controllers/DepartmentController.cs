using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using System.Data.SqlClient;
using System.Net;
using System.Diagnostics;

namespace HospitalProjectTeamThree.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
       

        [HttpPost]
        public ActionResult Add(string DepartmentName)
        {

            string query = "insert into Departments (DepartmentName) values (@DepartmentName)";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@DepartmentName", DepartmentName);
            db.Database.ExecuteSqlCommand(query, sqlparams);


            return RedirectToAction("List");
        }



        public ActionResult Add()
        {
            return View();
        }

        //  [Authorize(Roles = "Admin,Editor")]
        public ActionResult List()

        {
            List<Department> Departments = db.Departments.SqlQuery("Select * from Departments").ToList();
            return View(Departments);
        }
        public ActionResult Update(int id)
        {
            Department selecteddepartment = db.Departments.SqlQuery("Select * from Departments where DepartmentId=@id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selecteddepartment);
        }

        [HttpPost]
        public ActionResult Update(int id, string DepartmentName)
        {
            string query = "Update Departments set DepartmentName=@DepartmentName where DepartmentId=@DepartmentId";
            
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@DepartmentName", DepartmentName);
            sqlparams[1] = new SqlParameter("@DepartmentId", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            
            return RedirectToAction("List");
        }
        public ActionResult ConfirmDelete(int id)
        {
            string query = "delete from Departments where DepartmentId=@id";
            SqlParameter sqlparams = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        // [Authorize(Roles = "Admin,Editor,Registered User")]
        public ActionResult Delete(int id)
        {
            string query = "select * from Departments where DepartmentId = @id";
            SqlParameter sqlparams = new SqlParameter("@id", id);
            Department selectedDepartment = db.Departments.SqlQuery(query, sqlparams).FirstOrDefault();

            return View(selectedDepartment);
        }
    }
}
