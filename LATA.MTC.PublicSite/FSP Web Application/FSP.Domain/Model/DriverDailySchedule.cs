using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(DriverDailyScheduleMetaData))]
    public partial class DriverDailySchedule
    {
    }

    public class DriverDailyScheduleMetaData
    {
        [Required]
        [DisplayName("Driver ID")]
        public Guid DriverID { get; set; }

        [Required]
        [DisplayName("Beat Schedule ID")]
        public Guid BeatScheduleID { get; set; }

        [Required]
        [DisplayName("Beat ID")]
        public Guid BeatID { get; set; }
    }
}
