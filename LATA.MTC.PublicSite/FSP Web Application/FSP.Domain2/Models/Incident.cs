using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Incident
    {
        public Incident()
        {
            this.Assists = new List<Assist>();
        }

        public System.Guid IncidentID { get; set; }
        public string ApproximateLocation { get; set; }
        public int FreewayID { get; set; }
        public Nullable<System.Guid> LocationID { get; set; }
        public System.Guid BeatSegmentID { get; set; }
        public Nullable<System.TimeSpan> TimeStamp { get; set; }
        public Nullable<System.DateTime> DateStamp { get; set; }
        public System.Guid CreatedBy { get; set; }
        public string Description { get; set; }
        public string IncidentNumber { get; set; }
        public System.DateTime LastModified { get; set; }
        public string Location { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreet2 { get; set; }
        public string Direction { get; set; }
        public string BeatNumber { get; set; }
        public string DispatchNumber { get; set; }
        public virtual ICollection<Assist> Assists { get; set; }
        public virtual Freeway Freeway { get; set; }
        public virtual Location Location1 { get; set; }
    }
}
