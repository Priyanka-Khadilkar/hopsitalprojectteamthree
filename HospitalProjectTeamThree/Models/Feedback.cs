using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalProjectTeamThree.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackFormId { get; set; }

        public string DoctorName { get; set; }

        public string FeedbackType { get; set; }

        public string Hygiene { get; set; }

        public string StaffKnowledge { get; set; }

        public string WaitTime { get; set; }

        public string Comments { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
