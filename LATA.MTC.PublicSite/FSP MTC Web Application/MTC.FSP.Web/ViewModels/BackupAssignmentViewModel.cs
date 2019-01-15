using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MTC.FSP.Web.Models;

namespace MTC.FSP.Web.ViewModels
{
    public class BackupAssignmentViewModel
    {
      
        public int Id { get; set; }

        [DisplayName("Back-up Beat ID")]
        public Guid BeatId { get; set; }

        [DisplayName("Back-up Beat Number")]
        public String BeatNumber { get; set; }
                     
        public int PrimaryBackupBeatId { get; set; }
        public String PrimaryBackupBeatNumber { get; set; }
        public Guid? PrimaryBackupContractorId { get; set; }
        public String PrimaryBackupContractorName { get; set; }

        public int SecondaryBackupBeatId { get; set; }
        public String SecondaryBackupBeatNumber { get; set; }
        public Guid? SecondaryBackupContractorId { get; set; }
        public String SecondaryBackupContractorName { get; set; }

        public int TertiaryBackupBeatId { get; set; }
        public String TertiaryBackupBeatNumber { get; set; }
        public Guid? TertiaryBackupContractorId { get; set; }
        public String TertiaryBackupContractorName { get; set; }

        public DateTime CreatedOn { get; set; }

        public String CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public String ModifiedBy { get; set; }

        
    }
}