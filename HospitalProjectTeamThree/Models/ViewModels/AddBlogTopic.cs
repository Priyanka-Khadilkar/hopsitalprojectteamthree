using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class AddBlogTopic
    {
        public DoctorsBlog Blog { get; set; }
        public List<DoctorsBlog> Blogs { get; set; }
        public BlogTopic Topics { get; set; }
        public List<BlogTopic> BlogTopics { get; set; }
        public virtual ApplicationUser User { get; set; }
        public List<BlogTopic> Add_Topic { get; set; }
    }
}