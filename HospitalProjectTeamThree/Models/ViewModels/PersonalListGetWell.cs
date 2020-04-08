using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class PersonalListGetWell
    {
        //We need a list of card
        public virtual List<GetWellSoonCard> GetWellSoonCard { get; set; }
        //a card design
        public virtual CardDesign CardDesign { get; set; }
        //1 user info
        public virtual ApplicationUser User { get; set; }
    }
}