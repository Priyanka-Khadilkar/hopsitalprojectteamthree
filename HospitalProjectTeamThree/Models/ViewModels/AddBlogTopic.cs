using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class AddBlogTopic
    {
        public DoctorsBlog Blog { get; set; }
        public List<BlogTopic> BlogTopics { get; set; }
        public List<BlogTopic> Add_Topic { get; set; }
    }
}