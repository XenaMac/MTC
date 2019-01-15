using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class ClientAssist
    {
        //When a tow truck comes across an incident on its own, it needs to be able to report it back to the system
        //but it will have to post incident and assist data at the same time, this object will then be broken into
        //assist and incident data and handled appropriately.
        //This is largely an input class coming from the tablet client.

        //Start modeling incident
        public Guid IncidentID { get; set; }
        public string Location { get; set; }
        public int FreewayID { get; set; }
        public Guid LocationID { get; set; }
        public Guid BeatSegmentID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string IncidentNumber { get; set; }
        public string Direction { get; set; }
        //Start modeling Assist
        public Guid AssistID { get; set; }
        //IncidentID pulled from above
        public Guid DriverID { get; set; }
        public Guid FleetVehicleID { get; set; }
        public DateTime DispatchTime { get; set; }
        public int CustomerWaitTime { get; set; }
        public Guid VehiclePositionID { get; set; }
        public Guid IncidentTypeID { get; set; }
        public Guid TrafficSpeedID { get; set; }
        public string DropZone { get; set; }
        public string Make { get; set; }
        public Guid VehicleTypeID { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }
        public double StartOD { get; set; }
        public double EndOD { get; set; }
        public Guid TowLocationID { get; set; }
        public string Tip { get; set; }
        public string TipDetail { get; set; }
        public string CustomerLastName { get; set; }
        public string Comments { get; set; }
        public bool IsMDC { get; set; }
        public DateTime x1097 { get; set; }
        public DateTime x1098 { get; set; }
        public DateTime OnSiteTime { get; set; }
        public Guid ContractorID { get; set; }
        public string LogNumber { get; set; }
        public string[] SelectedServices { get; set; }
        public string CrossStreet1 { get; set; }
        public string CrossStreet2 { get; set; }
        public string SurveyNum { get; set; }
        public string BeatNumber { get; set; }
    }
}