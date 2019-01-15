using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(ServiceTypenMetaData))]
    public partial class ServiceType
    {
    }

    public class ServiceTypenMetaData
    {
        [Required]
        [DisplayName("Service Type Code")]
        public string ServiceTypeCode { get; set; }

        [Required]
        [DisplayName("Service Type Name")]
        public string ServiceType1 { get; set; }

    }
}
