using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(BeatScheduleMetaData))]
    public partial class BeatSchedule
    {
    }

    public class BeatScheduleMetaData
    {
        [Required]
        [DisplayName("Beat Schedule ID")]
        public Guid BeatScheduleID { get; set; }

        [DisplayName("Schedule Name")]
        [Required]
        [StringLength(50)]
        public String ScheduleName { get; set; }

        [DisplayName("Is Week Day?")]
        [Required]
        public bool Weekday { get; set; }

        [DisplayName("Log On time")]
        [Required]
        public TimeSpan Logon { get; set; }

        [DisplayName("Log Off time")]
        [Required]
        public TimeSpan LogOff { get; set; }

        [DisplayName("Roll Out time")]
        [Required]
        public TimeSpan RollOut { get; set; }

        [DisplayName("Roll In time")]
        [Required]
        public TimeSpan RollIn { get; set; }

        [DisplayName("On Patrol time")]
        [Required]
        public TimeSpan OnPatrol { get; set; }
    }
}
