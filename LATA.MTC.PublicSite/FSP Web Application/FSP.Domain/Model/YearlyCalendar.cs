using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(YearlyCalendarMetaData))]
    public partial class YearlyCalendar
    {
    }

    public class YearlyCalendarMetaData
    {       
        [DisplayName("Day Name")]
        [Required]
        [StringLength(100)]
        public String dayName { get; set; }

        [DisplayName("Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public int Date { get; set; }
    }
}
