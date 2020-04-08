using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class AddGetWellCard
    {
        //We need a list of card design to put in radio box
        public virtual List<CardDesign> CardDesigns { get; set; }
        //A userId who create the card
        public virtual ApplicationUser User { get; set; }
    }
}