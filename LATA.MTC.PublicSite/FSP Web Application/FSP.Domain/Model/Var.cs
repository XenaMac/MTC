using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(VarMetaData))]
    public partial class Var
    {
    }

    public class VarMetaData
    {
        [Required]
        [DisplayName("Variable ID")]
        public Guid VarID { get; set; }

        [DisplayName("Variable Name")]
        [Required]
        [StringLength(50)]
        public String VarName { get; set; }

        [DisplayName("Variable Value")]
        [Required]
        [StringLength(200)]
        public String VarValue { get; set; }
        
    }
}
