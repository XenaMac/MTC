using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(TowLocationMetaData))]
    public partial class TowLocation
    {
    }

    public class TowLocationMetaData
    {
        [Required]
        [DisplayName("Tow Location Code")]
        public string TowLocationCode { get; set; }

        [Required]
        [DisplayName("Tow Location Name")]
        public string TowLocation1 { get; set; }

    }
}
