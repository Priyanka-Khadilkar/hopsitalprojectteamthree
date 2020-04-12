using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using HospitalProjectTeamThree.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProjectTeamThree.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        //name of the Article
        public string ArticleTitle { get; set; }
        //Article author
        public string ArticleAuthor { get; set; }
        //Content of the article
        public string ArticleContent { get; set; }
        //posted date will be automatically added on submition
        public DateTime DatePosted { get; set; }

        

    }
}