//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTC.FSP.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MTCPreAssist
    {
        public System.Guid PreAssistID { get; set; }
        public System.Guid IncidentID { get; set; }
        public string Direction { get; set; }
        public string FSPLocation { get; set; }
        public string DispatchLocation { get; set; }
        public string Position { get; set; }
        public string LaneNumber { get; set; }
        public string CHPIncidentType { get; set; }
        public string CHPLogNumber { get; set; }
        public string IncidentSurveyNumber { get; set; }
        public Nullable<double> Lat { get; set; }
        public Nullable<double> Lon { get; set; }
        public Nullable<System.DateTime> datePosted { get; set; }
        public string DispatchCode { get; set; }
        public string Comments { get; set; }
        public string CrossStreet { get; set; }
        public string Freeway { get; set; }
        public string DriverLastName { get; set; }
        public string DriverFirstName { get; set; }
        public Nullable<System.Guid> DriverID { get; set; }
        public Nullable<System.Guid> RunID { get; set; }
    
        public virtual Driver Driver { get; set; }
        public virtual MTCIncident MTCIncident { get; set; }
    }
}
