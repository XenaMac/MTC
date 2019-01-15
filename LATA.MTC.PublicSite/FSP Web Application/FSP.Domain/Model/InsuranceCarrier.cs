using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(InsuranceCarrierMetaData))]
    public partial class InsuranceCarrier
    {
    }

    public class InsuranceCarrierMetaData
    {
        [Required]
        [DisplayName("Carrier Name")]
        public string CarrierName { get; set; }

        [Required]
        [DisplayName("Insurance Policy Number")]
        public string InsurancePolicyNumber { get; set; }

        [Required]
        [DisplayName("Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [DisplayName("Policy Name")]
        public string PolicyName { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Fax Number")]
        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }

        [Required]
        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }

}
