using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metra.Module.ServiceAlerts.Models
{
    public class ParsedMessage
    {
        public ParsedMessage()
        {
            AlertSummary = "";
            AlertText = "";
        }

        public String AlertSummary { get; set; }
        public String AlertText { get; set; }
    }
}
