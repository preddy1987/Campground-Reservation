using System;
using System.Collections.Generic;
using System.Text;

namespace NatParkCampRes.Models
{
    class Park
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Desc { get; set; }       
    }
}
