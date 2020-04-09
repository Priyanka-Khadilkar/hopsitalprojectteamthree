using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ListGetWell
    {
        //We need a single card design that belong to the card
        public virtual CardDesign CardDesign { get; set; }
       //showing all the card
        public virtual List<GetWellSoonCard> GetWellSoonCard { get; set; }
    }
}