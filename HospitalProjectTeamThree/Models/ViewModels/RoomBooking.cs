using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class RoomBooking
    {
        //need info about one room
        public virtual Room Room { get; set; }
        //need list of all rooms
        public List<Room> Rooms { get; set; }
        //need user information 
        public virtual ApplicationUser User { get; set; }
    }
}