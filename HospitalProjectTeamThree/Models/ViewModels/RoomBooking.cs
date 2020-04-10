using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class RoomBooking
    {
        public virtual Room Room { get; set; }
        public List<Room> Rooms { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}