using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class Incident
    {
        public Guid IncidentID { get; set; }
        public string Direction { get; set; }
        public string Location { get; set; }
        public int FreewayID { get; set; }
        public Guid LocationID { get; set; }
        public Guid BeatSegmentID { get; set; }
        public DateTime TimeStamp { get; set; } //break into TimeStamp(time) and Datestamp(date) for push to db
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string IncidentNumber { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreet2 { get; set; }
        public string BeatNumber { get; set; }
        //public string DispatchNumber { get; set; }
    }
}