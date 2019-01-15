using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(InspectionTypeMetaData))]
    public partial class InspectionType
    {
    }

    public class InspectionTypeMetaData
    {
        [Required]
        [DisplayName("Inspection Type Code")]
        public string InspectionTypeCode { get; set; }

        [Required]
        [DisplayName("Inspection Type Name")]
        public string InspectionType1 { get; set; }
    }
}
