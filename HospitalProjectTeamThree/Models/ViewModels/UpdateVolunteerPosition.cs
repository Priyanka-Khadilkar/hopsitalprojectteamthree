using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;


namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class UpdateVolunteerPosition
    {
        public virtual VolunteerPosition VolunteerPosition { get; set; }
        public List<Department> Departments { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}