using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class InspectionType
    {
        public InspectionType()
        {
            this.CHPInspections = new List<CHPInspection>();
        }

        public System.Guid InspectionTypeID { get; set; }
        public string InspectionType1 { get; set; }
        public string InspectionTypeCode { get; set; }
        public virtual ICollection<CHPInspection> CHPInspections { get; set; }
    }
}
