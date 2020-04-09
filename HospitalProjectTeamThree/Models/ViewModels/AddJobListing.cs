using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class AddJobListing
    {
        public JobListing JobListing { get; set; }
        public List<Department> Departments { get; set; }
        
        public virtual ApplicationUser User { get; set; }
    }
}