using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Assist
    {
        public System.Guid AssistID { get; set; }
        public System.Guid IncidentID { get; set; }
        public System.Guid DriverID { get; set; }
        public System.Guid FleetVehicleID { get; set; }
        public Nullable<System.DateTime> DispatchTime { get; set; }
        public Nullable<int> CustomerWaitTime { get; set; }
        public Nullable<System.Guid> VehiclePositionID { get; set; }
        public Nullable<System.Guid> IncidentTypeID { get; set; }
        public Nullable<System.Guid> TrafficSpeedID { get; set; }
        public Nullable<System.Guid> ServiceTypeID { get; set; }
        public string DropZone { get; set; }
        public string Make { get; set; }
        public Nullable<System.Guid> VehicleTypeID { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string State { get; set; }
        public Nullable<double> StartOD { get; set; }
        public Nullable<double> EndOD { get; set; }
        public Nullable<System.Guid> TowLocationID { get; set; }
        public string Tip { get; set; }
        public string TipDetail { get; set; }
        public string CustomerLastName { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> IsMDC { get; set; }
        public Nullable<System.DateTime> x1097 { get; set; }
        public Nullable<System.DateTime> x1098 { get; set; }
        public System.Guid ContractorID { get; set; }
        public string LogNumber { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public Nullable<double> Lat { get; set; }
        public Nullable<double> Lon { get; set; }
        public Nullable<System.DateTime> OnSiteTime { get; set; }
        public string SurveyNum { get; set; }
        public string AssistNumber { get; set; }
        public virtual Incident Incident { get; set; }
    }
}
