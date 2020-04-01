using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int RoomTotalBeds { get; set; }
        public int RoomPrice { get; set; } //in cents(e.g. 10 dollars = 1000 cents)
    }
}