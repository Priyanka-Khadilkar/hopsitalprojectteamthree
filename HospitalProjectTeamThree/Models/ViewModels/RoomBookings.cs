using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class RoomBookings
    {
        //this viewmodel allows to display information about rooms, user, and the booking
        //Info about one room
        public virtual Room Room { get; set; }
        //List of all rooms
        public List<Room> Rooms { get; set; }
        //User information 
        public virtual ApplicationUser User { get; set; }
        //Information from RoomBookings table
        public virtual RoomBookings RoomBooking { get; set; }
        public virtual RoomBooking AllRoomBookings { get; set; }
        

    }
}