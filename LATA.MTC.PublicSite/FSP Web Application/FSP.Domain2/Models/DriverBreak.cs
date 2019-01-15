using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class DriverBreak
    {
        public System.Guid BreakID { get; set; }
        public System.Guid DriverID { get; set; }
        public string BreakType { get; set; }
        public System.DateTime DateOfEvent { get; set; }
        public int Duration { get; set; }
    }
}
