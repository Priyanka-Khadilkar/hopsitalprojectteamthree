using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowVolunteerPosition
    {
        public virtual VolunteerPosition VolunteerPosition { get; set; }
        public virtual List<Department> Departments { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
    }
}