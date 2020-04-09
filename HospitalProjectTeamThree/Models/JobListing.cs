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
    public class JobListing
    {
        /*
         *  A job listing is a post that can be written by an admin or editor. 
         *  A job listing is a post that can be read by a user, guest, or admin
         *  
         *  A job listing can be written by one person (one user that's an admin)
         *  A job listing can belong to one department
         *  
         *  A job listing is written by one person.  A person can write many job listings
         *  A job listing can only be for one department.  A department can have many job listings
         *  
         *  Things that describe a job listing:
         *  -title
         *  -job description
         *  -start date
         *  -salary
         *  -is it published or not
         *  -an 'author'/admin/writer
         *  -what department it belongs to
         *  
         *  A job listing must reference a department
         *  A job listing must reference a user
         *  
         * 
         */

        [Key]
        public int JobID { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Salary { get; set; }
        public DateTime StartDate { get; set; }
        public string Published { get; set; }
        public int DepartmentID { get; set; }
        [ForeignKey("DepartmentID")]
        public virtual Department Department { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

    }
}