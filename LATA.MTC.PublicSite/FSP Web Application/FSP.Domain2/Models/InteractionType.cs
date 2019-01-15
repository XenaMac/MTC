using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class InteractionType
    {
        public InteractionType()
        {
            this.DriverInteractions = new List<DriverInteraction>();
        }

        public System.Guid InteractionTypeID { get; set; }
        public string InteractionType1 { get; set; }
        public virtual ICollection<DriverInteraction> DriverInteractions { get; set; }
    }
}
