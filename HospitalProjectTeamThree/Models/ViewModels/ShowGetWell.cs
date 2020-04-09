using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowGetWell
    {
        //We need a card design belong to the card
        public virtual CardDesign CardDesign { get; set; }
        //We need a card
        public virtual GetWellSoonCard GetWellSoonCard { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}