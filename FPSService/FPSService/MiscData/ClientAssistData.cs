using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class ClientAssistData
    {
        public Guid IncidentID { get; set; }
        public Guid AssistID { get; set; }
        public string Direction { get; set; }
        public string Freeway { get; set; }
        public string Location { get; set; }
        public string CrossStreet { get; set; }
        public string Comments { get; set; }
    }
}