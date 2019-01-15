using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(ContractorManagerMetaData))]
    public partial class ContractorManager
    {
    }

    public class ContractorManagerMetaData
    {
       
        [Required]
        [DisplayName("First Name")]
        [StringLength(50)]
        public String FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public String LastName { get; set; }

        [Required]
        [DisplayName("PhoneNumber")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }

        [Required]
        [DisplayName("Email")]
        [StringLength(75)]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Cell Phone")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public String CellPhone { get; set; }

        [Required]
        [DisplayName("Contractor")]
        public Guid ContractorID { get; set; }
    }
}
