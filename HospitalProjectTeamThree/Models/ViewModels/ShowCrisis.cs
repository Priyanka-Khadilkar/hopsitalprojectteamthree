using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowCrisis
    {
        public virtual Article article { get; set; }

        public List<Crisis> crises { get; set; }
    }
}