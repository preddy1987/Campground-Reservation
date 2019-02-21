using System;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampRes.Models
{
    public class SiteReservation : Site
    {
        public string Name { get; set; }
        public decimal DailyFee { get; set; }
    }
}
