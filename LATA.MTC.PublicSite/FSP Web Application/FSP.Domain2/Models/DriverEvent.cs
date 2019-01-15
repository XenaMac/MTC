using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class DriverEvent
    {
        public System.Guid ID { get; set; }
        public System.Guid DriverID { get; set; }
        public string EventType { get; set; }
        public System.DateTime TimeStamp { get; set; }
    }
}
