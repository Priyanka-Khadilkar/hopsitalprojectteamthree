using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class UpdateGetWell
    {
        //To update:
        //We need ALL the card designs
        public virtual ICollection<CardDesign> CardDesigns { get; set; }
        //And information about one card
        public virtual GetWellSoonCard GetWellSoonCard { get; set; }
    }
}