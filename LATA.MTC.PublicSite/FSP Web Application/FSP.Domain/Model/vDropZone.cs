using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(vDropZoneMetaData))]
    public partial class vDropZone
    {
    }

    public class vDropZoneMetaData
    {
        [Required]
        [DisplayName("Location")]
        [StringLength(100)]
        public string Location { get; set; }

        [Required]
        [DisplayName("Comments")]
        [StringLength(500)]
        public string Comments { get; set; }

        [Required]
        [DisplayName("Restrictions")]
        [StringLength(500)]
        public string Restrictions { get; set; }

        [Required]
        [DisplayName("Drop Zone Number")]
        [StringLength(50)]
        public string DropZoneNumber { get; set; }

        [Required]
        [DisplayName("Drop Zone Description")]
        [StringLength(500)]
        public string DropZoneDescription { get; set; }

        [Required]
        [DisplayName("City")]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [DisplayName("PD Phone Number")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PDPhoneNumber { get; set; }

        [Required]
        [DisplayName("Capacity")]       
        public int Capacity { get; set; }

    }
}
