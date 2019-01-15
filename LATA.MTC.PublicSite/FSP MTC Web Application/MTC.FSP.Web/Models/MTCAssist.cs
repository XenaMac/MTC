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
    
    public partial class MTCAssist
    {
        public MTCAssist()
        {
            this.MTCActionTakens = new HashSet<MTCActionTaken>();
        }
    
        public System.Guid MTCAssistID { get; set; }
        public System.Guid IncidentID { get; set; }
        public string TrafficCollision { get; set; }
        public string DebrisOnly { get; set; }
        public string Breakdown { get; set; }
        public string Other { get; set; }
        public string ProblemNote { get; set; }
        public string OtherNote { get; set; }
        public string TransportType { get; set; }
        public Nullable<double> StartODO { get; set; }
        public Nullable<double> EndODO { get; set; }
        public string DropSiteBeat { get; set; }
        public string DropSite { get; set; }
        public string State { get; set; }
        public string LicensePlateNumber { get; set; }
        public string VehicleType { get; set; }
        public string OTAuthorizationNumber { get; set; }
        public string DetailNote { get; set; }
        public Nullable<double> Lat { get; set; }
        public Nullable<double> Lon { get; set; }
        public string DropSiteOther { get; set; }
        public Nullable<System.DateTime> datePosted { get; set; }
        public string CallSign { get; set; }
        public string TimeOnInc { get; set; }
        public string TimeOffInc { get; set; }
    
        public virtual ICollection<MTCActionTaken> MTCActionTakens { get; set; }
        public virtual MTCIncident MTCIncident { get; set; }
    }
}