using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class FeaturedRoom
    {
        public virtual Room room { get; set; }
        public List<Room> rooms { get; set; }


    }
}