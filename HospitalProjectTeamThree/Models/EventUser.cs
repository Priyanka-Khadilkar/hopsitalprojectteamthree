using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models
{
    public class EventUser
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}