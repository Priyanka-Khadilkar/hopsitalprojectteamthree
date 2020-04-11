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
    public class GetWellSoonCard
    {
        [Key]
        public int CardId { get; set; }
        public string Message { get; set; }
        //this will be substituted by a separate card design table
        //one card has 1 design, 1 design can be apply to many cards
        //public string CardDesignNumber { get; set; }
        //this alone inject the userid into the table
        public virtual ApplicationUser User { get; set; }
        public string PatientName { get; set; }
        public string RoomNumber { get; set; }
        public string PatientEmail { get; set; }

        //Allow it to be null first by ?
        public int? CardDesignId { get; set; }
        [ForeignKey("CardDesignId")]
        public virtual CardDesign CardDesign { get; set; }

        /*public string CardUserId { get; set; }
        [ForeignKey("CardUserId")]*/
        //public virtual ApplicationUser Users { get; set; }
    }
}