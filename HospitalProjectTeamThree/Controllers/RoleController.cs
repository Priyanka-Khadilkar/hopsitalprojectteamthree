using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Data;
using Microsoft.AspNet.Identity.Owin;

namespace HospitalProjectTeamThree.Controllers
{
    public class RoleController : Controller
    {

        private ApplicationRoleManager _roleManager;


        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager roleManager)
        {
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
        public ActionResult Index()
        {
           return View();
        }
        // GET: Role
        [Authorize(Roles = "Admin")]
        //this only let admin access and create roles
        public ActionResult List()
        {
            //return a list of role
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
            list.Add(new RoleViewModel(role));
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        //this only let admin access and create roles
        //display the adding page

        public ActionResult Add()
        {
            return View();
        }
        //add the role to the database
        [HttpPost]
        public async Task<ActionResult> Add(RoleViewModel model)
        {
            //take in the Name as a parameter
            var role = new ApplicationRole() { Name = model.RoleName };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        //this only let admin access and create roles
        //To display Edit we need the id of the role
        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            //return the speicific role having the id
            return View(new RoleViewModel(role));
        }
        [HttpPost]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            var role = new ApplicationRole() { Id = model.Id, Name = model.RoleName };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        //this only let admin access and create roles
        public async Task<ActionResult> Show(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            //return the speicific role having the id
            return View(new RoleViewModel(role));
        }
        [Authorize(Roles = "Admin")]
        //this only let admin access and create roles
        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id, string Name)
        {
            var role = await RoleManager.FindByIdAsync(id);
            Debug.WriteLine("I am trying to delete" + id);
            await RoleManager.DeleteAsync(role);
            return RedirectToAction("List");
        }

    }
}