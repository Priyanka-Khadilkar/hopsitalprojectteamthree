using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class RoomBookings
    {
        //need info about one room
        public virtual Room Room { get; set; }
        //need list of all rooms
        public List<Room> Rooms { get; set; }
        //need user information 
        public virtual ApplicationUser User { get; set; }
        //need information from RoomBookings table
        public virtual RoomBookings RoomBooking { get; set; }
        public virtual RoomBooking AllRoomBookings { get; set; }
        //public List<RoomBooking> AllBookings { get; set; }

    }
}