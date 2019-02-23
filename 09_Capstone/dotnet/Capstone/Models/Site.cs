using System;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampRes.Models
{
    public class Site 
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupants { get; set; }
        public bool IsAccessible { get; set; }
        public int MaxRvLength { get; set; }
        public bool HasUtilities { get; set; }
    }
}
