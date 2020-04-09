using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalProjectTeamThree.Data;


namespace HospitalProjectTeamThree.Models
{
    public class DoctorsBlog
    {
        [Key]
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public DateTime BlogDate { get; set; }
        public string BlogContent { get; set; }
        public string BlogSource { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<BlogTopic> Topics { get; set; }

    }
}