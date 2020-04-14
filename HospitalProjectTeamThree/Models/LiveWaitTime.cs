using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalProjectTeamThree.Data;
using System.ComponentModel;

namespace HospitalProjectTeamThree.Models
{
    public class LiveWaitTime
    {
        [Key]
        public int WaitUpdateId { get; set; }
        public DateTime WaitUpdateDate { get; set; }
        public DateTime WaitUpdateTime { get; set; }

        public int CurrentWaitTime { get; set; }
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }


    public enum WaitTimeDesc { Low = 1, Medium = 2, High = 3 }
}