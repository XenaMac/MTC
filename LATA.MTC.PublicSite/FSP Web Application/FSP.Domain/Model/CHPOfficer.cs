using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(CHPOfficerMetaData))]
    public partial class CHPOfficer
    {
    }

    public class CHPOfficerMetaData
    {
        [Required]
        [DisplayName("Badge ID")]
        [StringLength(50)]
        public String BadgeID { get; set; }


        [Required]
        [DisplayName("Officer First Name")]
        [StringLength(50)]
        public String OfficerFirstName { get; set; }

        [Required]
        [DisplayName("Officer Last Name")]
        [StringLength(50)]
        public String OfficerLastName { get; set; }
    }
}
