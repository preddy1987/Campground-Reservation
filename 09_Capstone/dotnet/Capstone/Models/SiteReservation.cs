using System;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampRes.Models
{
    public class SiteReservation : Site
    {
        public int SiteReservationConflict { get; set; }
    }
}
