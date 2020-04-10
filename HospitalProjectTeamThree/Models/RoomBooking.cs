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
        bool PaymentCleared { get; set; }
        string DateFrom { get; set; }
        string DateTo { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}