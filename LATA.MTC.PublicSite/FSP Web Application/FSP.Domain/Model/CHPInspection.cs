using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(CHPInspectionMetaData))]
    public partial class CHPInspection
    {
    }

    public class CHPInspectionMetaData
    {
        [Required]
        [DisplayName("Fleet Vehicle")]        
        public Guid FleetVehicleID { get; set; }

        [Required]
        [DisplayName("CHP Officer")]
        [StringLength(50)]
        public String BadgeID { get; set; }

        [Required]
        [DisplayName("Inspection Type")]
        public Guid InspectionTypeID { get; set; }

        [Required]
        [DisplayName("Contractor")]
        public Guid ContractorID { get; set; }

        [Required]
        [DisplayName("Inspection Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InspectionDate { get; set; }

        [Required]
        [DisplayName("Inspection Notes")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public String InspectionNotes { get; set; }
      
    }
}
