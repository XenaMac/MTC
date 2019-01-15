using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Contract
    {
        public System.Guid ContractID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal MaxObligation { get; set; }
        public string AgreementNumber { get; set; }
        public System.Guid BeatID { get; set; }
        public System.Guid ContractorID { get; set; }
        public virtual Beat Beat { get; set; }
        public virtual Contractor Contractor { get; set; }
    }
}
