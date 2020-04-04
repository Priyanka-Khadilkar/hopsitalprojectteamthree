using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models
{
    public class RoleViewModel
    {
        //this is the role table model
        //adding a model to transfer information to the view
        public RoleViewModel () { }
        
        public RoleViewModel (ApplicationRole role)
        {
            Id = role.Id;
            RoleName = role.Name;
        }
        public string Id { get; set; }
        public string RoleName { get; set; }
    }
}