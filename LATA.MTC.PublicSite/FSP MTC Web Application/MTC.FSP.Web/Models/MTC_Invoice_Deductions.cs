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
    
    public partial class MTC_Invoice_Deductions
    {
        public System.Guid DeductionID { get; set; }
        public string InvoiceID { get; set; }
        public string Category { get; set; }
        public System.DateTime Date { get; set; }
        public string Description { get; set; }
        public int TimeAdded { get; set; }
        public double Rate { get; set; }
        public double Cost { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual MTC_Invoice MTC_Invoice { get; set; }
    }
}
