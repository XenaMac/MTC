using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class InvoiceAdditions
    {
        public Guid id { get; set; }
        public string category { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public int TimeAdded { get; set; }
        public decimal Rate { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.##}")]
        public decimal Cost { get; set; }
        public string shiftDay { get; set; }
        public int MENum { get; set; }
    }
}