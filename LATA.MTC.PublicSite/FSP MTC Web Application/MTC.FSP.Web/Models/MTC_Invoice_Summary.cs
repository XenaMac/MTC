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
    
    public partial class MTC_Invoice_Summary
    {
        public System.Guid SummaryID { get; set; }
        public string InvoiceID { get; set; }
        public string Row { get; set; }
        public double Days { get; set; }
        public double Shifts { get; set; }
        public double ContractHours { get; set; }
        public double OnPatrolHours { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual MTC_Invoice MTC_Invoice { get; set; }
    }
}
