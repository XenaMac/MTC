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
    
    public partial class TruckMessage
    {
        public System.Guid TruckMessageID { get; set; }
        public string TruckIP { get; set; }
        public string MessageText { get; set; }
        public System.DateTime SentTime { get; set; }
        public bool Acked { get; set; }
        public Nullable<System.DateTime> AckedTime { get; set; }
        public string UserEmail { get; set; }
        public string TruckNumber { get; set; }
        public string CallSign { get; set; }
        public string DriverName { get; set; }
        public string BeatNumber { get; set; }
        public string Response { get; set; }
    }
}
