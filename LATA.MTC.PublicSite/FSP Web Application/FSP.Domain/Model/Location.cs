using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(LocationMetaData))]
    public partial class Location
    {
    }

    public class LocationMetaData
    {
        [Required]
        [DisplayName("Location Code")]
        public string LocationCode { get; set; }

        [Required]
        [DisplayName("Location Name")]
        public string Location1 { get; set; }

    }


}
