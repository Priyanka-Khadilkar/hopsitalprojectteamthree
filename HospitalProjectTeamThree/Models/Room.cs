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
        public int RoomNumber { get; set; }

        //I realized Room Type should have been separate table
        public string RoomType { get; set; }
        public int RoomTotalBeds { get; set; }
        public int RoomPrice { get; set; } //in cents(e.g. 10 dollars = 1000 cents)
        public string RoomDesc { get; set; }

        //Room may have many users associated with room (Many bookings)
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<RoomBooking> RoomBookings { get; set; }

    }
}