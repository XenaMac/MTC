using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class IncidentView //Slightly optimized view of incident, used for returning to dashboard screen, guid replaced with strings
    {
        public Guid IncidentID { get; set; }
        public string Direction { get; set; }
        public string Freeway { get; set; }
        public string Location { get; set; }
        public string BeatNumber { get; set; }
        public string TimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string IncidentNumber { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreet2 { get; set; }
        public string DispatchNumber { get; set; }
    }
}