using FSP.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSP.Web.Areas.AdminArea.ViewModels
{
    public class ContractCreateViewModel
    {       
        public Contract Contract { get; set; }
        public List<Guid> SelectedBeats { get; set; }
    }
}