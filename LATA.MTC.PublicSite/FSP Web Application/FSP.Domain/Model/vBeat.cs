using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(vBeatMetaData))]
    public partial class vBeat
    {
    }

    public class vBeatMetaData
    {
        [Required]
        [DisplayName("Beat ID")]
        public Guid BeatID { get; set; }

        [DisplayName("Active")]
        [Required]
        public Boolean Active { get; set; }

        [DisplayName("Freeway")]
        [Required]
        public int FreewayID { get; set; }

        [DisplayName("Beat Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public int BeatDescription { get; set; }

        [DisplayName("Beat Number")]
        [StringLength(50)]
        public int BeatNumber { get; set; }

        [DisplayName("Last Update")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastUpdate { get; set; }

        [DisplayName("Last Update By")]
        [StringLength(50)]
        public int LastUpdateBy { get; set; }

        [DisplayName("Is Temporary")]
        public Boolean IsTemporary { get; set; }

        [DisplayName("Beat Color")]
        [StringLength(50)]
        public String BeatColor { get; set; }
    }
}
