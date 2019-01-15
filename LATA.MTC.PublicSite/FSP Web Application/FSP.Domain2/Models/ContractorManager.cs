using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class ContractorManager
    {
        public System.Guid ContractorManagerID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.Guid ContractorID { get; set; }
        public virtual Contractor Contractor { get; set; }
    }
}
