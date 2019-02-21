using System;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampRes.Models
{
    public class Campground : BaseItem
    {
        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpenFromMonth { get; set; }
        public int OpenToMonth { get; set; }
        public decimal DailyFee { get; set; }
    }
}
