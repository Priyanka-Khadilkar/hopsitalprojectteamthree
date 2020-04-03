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
        public string CrisisName { get; set; }
        public string CrisisDesc { get; set; }
        public DateTime CrisisStrated { get; set; }
        public DateTime CrisisFinished { get; set; }

        public ICollection<Article> Articles { get; set; }



    }
}