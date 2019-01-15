using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class PortableAssistRequest
    {
        public Guid IncidentID { get; set; }
        public Guid AssistID { get; set; }
        public DateTime DispatchTime { get; set; }
        public string AssistType { get; set; }
        public string BeatBoundry { get; set; }
        public string AssistInfo { get; set; }
        public string CreatedByID { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public string Freeway { get; set; }
        public string Location { get; set; }
        public string CrossStreet { get; set; }
        public string Comments { get; set; }
    }
}