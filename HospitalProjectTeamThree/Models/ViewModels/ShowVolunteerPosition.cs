﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowVolunteerPosition
    {
        public virtual VolunteerPosition VolunteerPosition { get; set; }
        public virtual List<Department> Departments { get; set; }
    }
}