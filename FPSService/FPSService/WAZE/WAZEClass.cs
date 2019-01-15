using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.WAZE
{
    public class WAZEClass
    {
        public string incidentID { get; set; }
        public DateTime creationtime { get; set; }
        public DateTime updatetime { get; set; }
        public wType type { get;set;}
        public wSubType subType { get; set; }
        public string description { get; set; }
        public Location location { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
    }

    public enum wType{
        ACCIDENT,
        CONSTRUCTION,
        HAZARD,
        ROAD_CLOSED
    }

    public enum wSubType {
        NO_SUBTYPE,
        ACCIDENT_MINOR,
        ACCIDENT_MAJOR,
        HAZARD_ON_ROAD,
        HAZARD_ON_SHOULDER,
        HAZARD_WEATHER,
        HAZARD_ON_ROAD_OBJECT,
        HAZARD_ON_ROAD_POT_HOLE,
        HAZARD_ON_ROAD_ROAD_KILL,
        HAZARD_ON_SHOULDER_CAR_STOPPED,
        HAZARD_ON_SHOULDER_ANIMALS,
        HAZARD_ON_ROAD_CONSTRUCTION,
        HAZARD_ON_ROAD_CAR_STOPPED,
        ROAD_CLOSED_HAZARD,
        ROAD_CLOSED_CONSTRUCTION
    }

    public class Location {
        public string street { get; set; }
        public string polyline { get; set; }
    }
}