using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowCrisis
    {
        //this viewmodel allows to display information about the crisis and related articles on the same page
        public virtual Article article { get; set; }
        public virtual Crisis crises { get; set; }
        //List of crisis and Articles
        public List<Crisis> listcrises { get; set; }
        public List<Article> articles { get; set; }

    }
}