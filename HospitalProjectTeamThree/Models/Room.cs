using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using HospitalProjectTeamThree.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProjectTeamThree.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        //Number of specific room
        public int RoomNumber { get; set; }

        //I realized Room Type should have been separate table
        public string RoomType { get; set; }
        //Information about total beds in the room.Can be used to see if any beds available to book 
        public int RoomTotalBeds { get; set; }
        //Price of the room
        public int RoomPrice { get; set; } //in cents(e.g. 10 dollars = 1000 cents)
        //Description of the room
        public string RoomDesc { get; set; }

        //Room may have many users associated with room (Many bookings)
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<RoomBooking> RoomBookings { get; set; }

    }
}