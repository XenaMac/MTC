using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(VehiclePositionMetaData))]
    public partial class VehiclePosition
    {
    }

    public class VehiclePositionMetaData
    {
        [Required]
        [DisplayName("Vehicle Position Code")]
        public string VehiclePositionCode { get; set; }

        [Required]
        [DisplayName("Vehicle Position")]
        public string VehiclePosition1 { get; set; }

    }
}
