using System.Collections.Generic;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class FAQViewModel
    {
        public FAQ FAQ { get; set; }

        public IEnumerable<FAQAnswer> FAQAnswer { get; set; }

        public bool CanDelete { get; set; }
    }
}