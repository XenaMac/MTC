using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{   
    [MetadataType(typeof(BeatBeatScheduleMetaData))]
    public partial class BeatBeatSchedule
    {
    }

    public class BeatBeatScheduleMetaData
    {
        [Required]
        [DisplayName("Beat")]
        public Guid BeatID { get; set; }        

        [Required]
        [DisplayName("Schedule")]
        public Guid BeatScheduleID { get; set; }        
       
    }
}
