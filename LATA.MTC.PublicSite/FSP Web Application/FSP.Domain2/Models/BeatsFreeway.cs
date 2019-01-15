using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class BeatsFreeway
    {
        public System.Guid BeatFreewayID { get; set; }
        public System.Guid BeatID { get; set; }
        public int FreewayID { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public virtual Beat Beat { get; set; }
        public virtual Freeway Freeway { get; set; }
    }
}
