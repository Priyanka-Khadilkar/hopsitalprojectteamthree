using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleAuthor { get; set; }
        public string ArticleContent { get; set; }
        public DateTime DatePosted { get; set; }
    }
}