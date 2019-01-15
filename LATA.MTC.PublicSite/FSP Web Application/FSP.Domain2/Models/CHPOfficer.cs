using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class CHPOfficer
    {
        public CHPOfficer()
        {
            this.CHPInspections = new List<CHPInspection>();
            this.CHPInspections1 = new List<CHPInspection>();
        }

        public string BadgeID { get; set; }
        public string OfficerLastName { get; set; }
        public string OfficerFirstName { get; set; }
        public virtual ICollection<CHPInspection> CHPInspections { get; set; }
        public virtual ICollection<CHPInspection> CHPInspections1 { get; set; }
    }
}
