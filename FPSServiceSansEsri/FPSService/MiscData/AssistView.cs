using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class AssistView //Slightly optimized view of incident, used for returning to dashboard screen, guid replaced with strings
    {
        public Guid AssistID { get; set; }
        public Guid IncidentID { get; set; }
        public string Driver { get; set; }
        public string FleetVehicle { get; set; }
        public string DispatchTime { get; set; }
        public int CustomerWaitTime { get; set; }
        public string VehiclePosition { get; set; }
        public string IncidentType { get; set; }
        public string TrafficSpeed { get; set; }
        public string ServiceType { get; set; }
        public string DropZone { get; set; }
        public string Make { get; set; }
        public string VehicleType { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }
        public double StartOD { get; set; }
        public double EndOD { get; set; }
        public string TowLocation { get; set; }
        public string Tip { get; set; }
        public string TipDetail { get; set; }
        public string CustomerLastName { get; set; }
        public string Comments { get; set; }
        public bool IsMDC { get; set; }
        public string x1097 { get; set; }
        public string x1098 { get; set; }
        public string Contractor { get; set; }
        public string LogNumber { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool Acked { get; set; }
        public string AssistNumber { get; set; }
    }
}