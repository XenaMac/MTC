using System;
using System.Collections.Generic;

namespace FSP.Domain2.Models
{
    public partial class Weblink
    {
        public System.Guid WeblinkID { get; set; }
        public string URL { get; set; }
        public string Description { get; set; }
    }
}
