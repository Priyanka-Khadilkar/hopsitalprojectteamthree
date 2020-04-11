using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models
{
    public class VolunteerPosition
    {

        [Key]
        public int VolunteerPositionID { get; set; }
        public string VolunteerPositionTitle { get; set; }

        public string VolunteerPositionDescription { get; set; }
        public DateTime StartDate { get; set; }

        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }

        // a volunteer position can have many volunteers(users) and users
        //can sign up for multiple positions
        // a volunteer position can reference a user


    }
}