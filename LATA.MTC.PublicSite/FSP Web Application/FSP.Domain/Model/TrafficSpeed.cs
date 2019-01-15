using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(TrafficSpeedMetaData))]
    public partial class TrafficSpeed
    {
    }

    public class TrafficSpeedMetaData
    {
        [Required]
        [DisplayName("Traffic Speed Code")]
        public string TrafficSpeedCode { get; set; }

        [Required]
        [DisplayName("Traffic Speed Name")]
        public string TrafficSpeed1 { get; set; }

    }
}
