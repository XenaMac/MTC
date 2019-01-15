using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class IncidentType
    {
        public Guid CHPIncidentTypeID { get; set; }
        public string CHPIncidentType { get; set; }
    }

    public class LocationAbbreviation
    {
        public int ID { get; set; }
        public string position { get; set; }
        public string abbreviation { get; set; }
    }

    public class Transportation
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }

}