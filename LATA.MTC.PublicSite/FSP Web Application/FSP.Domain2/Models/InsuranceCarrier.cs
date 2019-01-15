using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class InsuranceCarrier
    {
        public InsuranceCarrier()
        {
            this.Contractors = new List<Contractor>();
        }

        public System.Guid InsuranceID { get; set; }
        public string CarrierName { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string PolicyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Contractor> Contractors { get; set; }
    }
}
