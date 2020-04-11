using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string EventFromTime { get; set; }
        public string EventToTime { get; set; }
        public string EventLocation { get; set; }
        public string EventTargetAudience { get; set; }
        public string EventHostedBy { get; set; }
        public string EventImagePath { get; set; }
        public string EventDetails { get; set; }
        public string EventCreatedBy { get; set; }
        [ForeignKey("EventCreatedBy")]
        public virtual ApplicationUser EventCreater { get; set; }
        public string EventUpdatedBy { get; set; }
        [ForeignKey("EventUpdatedBy")]
        public virtual ApplicationUser EventUpdater { get; set; }
        public DateTime EventCreatedOn { get; set; }
        public DateTime? EventUpdatedOn { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}