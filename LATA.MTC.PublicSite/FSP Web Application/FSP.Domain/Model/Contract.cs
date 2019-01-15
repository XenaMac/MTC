using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(ContractMetaData))]
    public partial class Contract
    {
    }

    public class ContractMetaData
    {       
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
        [DisplayName("Max Obligation")]
        public int MaxObligation { get; set; }

        [Required]
        [DisplayName("Agreement Number")]
        [StringLength(50)]
        public Decimal AgreementNumber { get; set; }

        //[Required]
        //[DisplayName("Beat")]
        //public Guid BeatID { get; set; }

        [Required]
        [DisplayName("Contractor")]
        public Guid ContractorID { get; set; }

    }
}
