using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class UpdateJobListing
    {

        public JobListing JobListing { get; set; }
        public List<Department> Departments { get; set; }
    }
}