using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class FeaturedRoom
    {
        //This viewmodel allows to display the room selected by the user, and list of all rooms available
        public virtual Room room { get; set; }
        //list of all rooms
        public List<Room> rooms { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}