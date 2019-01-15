using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{

    [MetadataType(typeof(IncidentTypeMetaData))]
    public partial class IncidentType{}

    public class IncidentTypeMetaData
    {
        [Required]
        [DisplayName("Incident Type Code")]
        public string IncidentTypeCode { get; set; }

        [Required]
        [DisplayName("Incident Type Name")]
        public string IncidentType1 { get; set; }
    }
}
