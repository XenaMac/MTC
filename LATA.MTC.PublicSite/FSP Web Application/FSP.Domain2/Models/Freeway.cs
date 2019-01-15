using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Freeway
    {
        public Freeway()
        {
            this.BeatsFreeways = new List<BeatsFreeway>();
            this.Incidents = new List<Incident>();
        }

        public int FreewayID { get; set; }
        public string FreewayName { get; set; }
        public virtual ICollection<BeatsFreeway> BeatsFreeways { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
    }
}
