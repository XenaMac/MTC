using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class TruckMessage
    {
        public System.Guid TruckMessageID { get; set; }
        public string TruckIP { get; set; }
        public string MessageText { get; set; }
        public System.DateTime SentTime { get; set; }
        public System.Guid UserID { get; set; }
        public bool Acked { get; set; }
        public Nullable<System.DateTime> AckedTime { get; set; }
    }
}
