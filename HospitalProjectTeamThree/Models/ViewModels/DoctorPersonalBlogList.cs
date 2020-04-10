using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class DoctorPersonalBlogList
    {
        public List<DoctorsBlog> Blog { get; set; }
        public List<BlogTopic> BlogTopics { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}