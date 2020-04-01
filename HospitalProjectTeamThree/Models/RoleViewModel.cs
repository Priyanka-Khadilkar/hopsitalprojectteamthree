using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models
{
    public class RoleViewModel
    {
        //adding a model to transfer information to the view
        public RoleViewModel () { }
        public RoleViewModel (ApplicationRole role)
        {
            Id = role.Id;
            Name = role.Name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}