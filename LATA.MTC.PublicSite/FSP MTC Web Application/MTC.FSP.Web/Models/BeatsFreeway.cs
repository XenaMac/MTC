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
    
    public partial class BeatsFreeway
    {
        public int BeatFreewayID { get; set; }
        public System.Guid BeatID { get; set; }
        public int FreewayID { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual BeatData BeatData { get; set; }
        public virtual Freeway Freeway { get; set; }
    }
}
