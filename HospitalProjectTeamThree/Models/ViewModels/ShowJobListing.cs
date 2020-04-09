using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowJobListing
    {
        public virtual JobListing JobListing { get; set; }
        public List<Department> Departments { get; set; }

    }
}