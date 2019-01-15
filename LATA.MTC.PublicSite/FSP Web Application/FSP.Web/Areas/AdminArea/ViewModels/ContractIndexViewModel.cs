using FSP.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSP.Web.Areas.AdminArea.ViewModels
{
    public class ContractIndexViewModel
    {
        public List<ContractIndex> Contracts { get; set; }
    }

    public class ContractIndex
    {
        public Contract Contract { get; set; }
        public List<String> SelectedBeats { get; set; }
    }


}