using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTC.FSP.Web.ViewModels
{
    public class BackupBeatViewModel
    {
        public int Id { get; set; }

        public Guid BeatId { get; set; }

        public String BeatNumber { get; set; }
       
        public DateTime CreatedOn { get; set; }

        public String CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public String ModifiedBy { get; set; }
    }
}