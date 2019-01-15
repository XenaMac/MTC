using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSP.Web.Areas.AdminArea.ViewModels
{
    public class BeatScheduleCRUDViewModel
    {
        public Guid BeatID { get; set; }
        public String BeatNumber { get; set; }
        public IList<SelectListItem> Schedules { get; set; }
    }
}