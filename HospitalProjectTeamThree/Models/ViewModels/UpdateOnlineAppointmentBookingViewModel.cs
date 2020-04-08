using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class UpdateOnlineAppointmentBookingViewModel
    {
        public OnlineAppointmentBooking OnlineAppointmentBooking { get; set; }

        public OnlineAppointmentBookingStatus OnlineAppointmentBookingStatus { get; set; }
    }
}