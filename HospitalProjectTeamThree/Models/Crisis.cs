using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProjectTeamThree.Models
{
    public class Crisis
    {

        [Key]
        public int CrisisId { get; set; }
        //title of the Crisis
        public string CrisisName { get; set; }
        //Short description about the crisis
        public string CrisisDesc { get; set; }
        //New crisis start date will be the day it was created
        public DateTime CrisisStrated { get; set; }

        //when administrator creates new crisis - word "No" will be inserted into database(To allow easy update, and use logic on main page)
        public string CrisisFinished { get; set; }

        //list of all articles
        public ICollection<Article> Articles { get; set; }



    }
}