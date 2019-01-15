using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPSService.DataClasses
{
    public class ClientIncidentType
    {
        public Guid IncidentTypeID { get; set; }
        public string IncidentTypeCode { get; set; }
        public string IncidentTypeName { get; set; }
    }

    public class ClientFreeway
    {
        public int FreewayID { get; set; }
        public string FreewayName { get; set; }
    }

    public class ClientServiceType
    {
        public Guid ServiceTypeID { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public class ClientDropZone
    {
        public Guid DropZoneID { get; set; }
        public string Location { get; set; }
    }

    public class ClientTowLocation
    {
        public Guid TowLocationID { get; set; }
        public string TowLocationCode { get; set; }
        public string TowLocationName { get; set; }
    }

    public class ClientContractor
    {
        public Guid ContractorID { get; set; }
        public string ContractCompanyName { get; set; }
    }

    public class ClientCode1098
    {
        public Guid CodeID { get; set; }
        public string CodeCall { get; set; }
    }

    public class ClientLocationCode
    {
        public Guid LocationID { get; set; }
        public string LocationCode { get; set; }
    }

    public class ClientTrafficSpeed
    {
        public Guid TrafficSpeedID { get; set; }
        public string TrafficSpeedCode { get; set; }
    }

    public class ClientVehiclePosition
    {
        public Guid VehiclePositionID { get; set; }
        public string VehiclePositionCode { get; set; }
    }

    public class ClientVehicleTypes
    {
        public Guid VehicleTypeID { get; set; }
        public string VehicleTypeCode { get; set; }
    }
}