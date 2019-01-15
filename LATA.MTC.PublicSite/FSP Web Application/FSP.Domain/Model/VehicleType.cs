using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(VehicleTypeMetaData))]
    public partial class VehicleType
    {
    }

    public class VehicleTypeMetaData
    {
        [Required]
        [DisplayName("Vehicle Type Code")]
        public string VehicleTypeCode { get; set; }

        [Required]
        [DisplayName("Vehicle Type Name")]
        public string VehicleType1 { get; set; }

    }
}
