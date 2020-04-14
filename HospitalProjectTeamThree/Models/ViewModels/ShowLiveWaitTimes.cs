using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HospitalProjectTeamThree.Data;

namespace HospitalProjectTeamThree.Models.ViewModels
{
    public class ShowLiveWaitTimes
    {
        public Department Departments { get; set; }
        public WaitTimeDesc WaitTimeList { get; set; }
        public LiveWaitTime WaitTime { get; set; }
        public List<LiveWaitTime> WaitTimes { get; set; }
        public List<Department> DepartmentsList { get; set; }
    }

}