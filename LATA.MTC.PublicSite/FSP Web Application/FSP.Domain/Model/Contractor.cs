using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(ContractorMetaData))]
    public partial class Contractor
    {
    }


    public class ContractorMetaData
    {

        [Required]
        [DisplayName("Contract Company Name")]
        [StringLength(100)]
        public string ContractCompanyName { get; set; }
            
        [Required]
        [DisplayName("Address")]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [DisplayName("Office Telephone")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20)]
        public string OfficeTelephone { get; set; }

        [Required]
        [DisplayName("MCP Number")]
        [StringLength(50)]
        public string MCPNumber { get; set; }

        [Required]
        [DisplayName("MCP Expiration")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public string MCPExpiration { get; set; }
       
        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Comments { get; set; }
           
        [Required]
        [DisplayName("City")]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [DisplayName("State")]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [DisplayName("Zip")]
        [StringLength(10)]
        public string Zip { get; set; }
    }
}
