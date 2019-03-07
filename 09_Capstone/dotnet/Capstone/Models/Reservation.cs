using System;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampRes.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Reservation
    { 
        /// <summary>
        /// 
        /// </summary>
        public int ReservationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SiteId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }


        public Reservation()
        {
            CreateDate = DateTime.Now;
        }
    }
}
