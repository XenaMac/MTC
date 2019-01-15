using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Group
    {
        public System.Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupCity { get; set; }
        public string GroupState { get; set; }
        public string GroupAddress { get; set; }
        public string GroupZip { get; set; }
        public string GroupPhone { get; set; }
        public string GroupContactName { get; set; }
    }
}
