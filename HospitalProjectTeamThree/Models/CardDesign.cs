using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProjectTeamThree.Models
{
    public class CardDesign
    {
        public int CardDesignId { get; set; }
        public string DesignName { get; set; }
        public int HasPic { get; set; }
        public string PicExt { get; set; }
        public ICollection<GetWellSoonCard> GetWellSoonCards { get; set; }

    }
}