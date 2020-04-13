using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models
{
    public class OnlineAppointmentBooking
    {
        [Key]
        public int OnlineAppointmentBookingId { get; set; }

        public DateTime PatientDateOfBirth { get; set; }

        public DateTime PreferredDate { get; set; }

        public string PreferredTime { get; set; }

        public string PreferredDoctor { get; set; }

        public int OnlineAppointmentBookingStatus { get; set; }

        public DateTime OnlineAppointmentBookingBookedOn { get; set; }

        //represents one to many relationship one user can book multiple appointment.
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }

    public enum OnlineAppointmentBookingStatus
    {
        //Enum for OnlineAppointment Status
        [Display(Name = "In Process")]
        InProcess = 1,
        Booked = 2,
        Cancelled = 3,
        Completed = 4
    }
}