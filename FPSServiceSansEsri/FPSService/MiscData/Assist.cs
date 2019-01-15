using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.MiscData
{
    public class Assist
    {
        public Guid AssistID { get; set; }
        public string AssistNumber { get; set; }
        public Guid IncidentID { get; set; }
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
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool Acked { get; set; }
        public string[] SelectedServices { get; set; }
        public bool AssistComplete { get; set; }
        public string SurveyNum { get; set; }
    }
}