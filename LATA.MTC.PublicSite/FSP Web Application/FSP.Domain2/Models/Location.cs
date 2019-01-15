using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Location
    {
        public Location()
        {
            this.Incidents = new List<Incident>();
        }

        public System.Guid LocationID { get; set; }
        public string LocationCode { get; set; }
        public string Location1 { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}
