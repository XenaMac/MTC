using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(FreewayMetaData))]
    public partial class Freeway
    {
    }

    public class FreewayMetaData
    {
        [Required]
        [DisplayName("Freeway Name")]
        public string FreewayName { get; set; }
     
    }
}
