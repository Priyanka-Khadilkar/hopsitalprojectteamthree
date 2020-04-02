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
    public class BlogTopic
    {
        [Key]
        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public ICollection<DoctorsBlog> DoctorsBlogs { get; set; }
    }
}