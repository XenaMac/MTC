using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
      [MetadataType(typeof(InteractionTypeMetaData))]
    public partial class InteractionType
    {
    }

    public class InteractionTypeMetaData
    {       
        [Required]
        [DisplayName("Interaction Type Name")]
        public string InteractionType1 { get; set; }
    }
}
