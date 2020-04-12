using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models
{
    public class RoomBooking
    {
        [Key]
        public int BookingId { get; set; }
                
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public int RoomID { get; set; }
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
        //for payments idea is to use third party processing services, and return will be 1 (cleared) or 0(not cleared) 
        public bool PaymentCleared { get; set; }
        //using string since I am going to have text inputs for the dates
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        //refers to list of all rooms
        public ICollection<Room> Rooms { get; set; }
    }
}