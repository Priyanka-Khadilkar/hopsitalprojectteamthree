using HospitalProjectTeamThree.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class RegisteredUserList
   {
        public List<ApplicationUser> UserList { get; set; }
        public Event EventData { get; set; }
    }
}