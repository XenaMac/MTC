using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(DriverMetaData))]
    public partial class Driver
    {
    }

    public class DriverMetaData
    {
        [Required]
        [DisplayName("Contractor")]
        public int ContractorID { get; set; }
           
        [DisplayName("Beat")]
        public Guid BeatID { get; set; }

        [Required]
        [DisplayName("First Name")]
        [StringLength(50)]
        public String FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public String LastName { get; set; }
       
        [DisplayName("Drivers License Number")]
        [StringLength(50)]
        public String DriversLicenseNumber { get; set; }

        [Required]
        [DisplayName("Password")]
        //[DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [DisplayName("FSPID Number")]
        [StringLength(50)]
        public String FSPIDNumber { get; set; }

        [Required]
        [DisplayName("Program Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProgramStartDate { get; set; }

        [DisplayName("Training Completion Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? TrainingCompletionDate { get; set; }

        [Required]
        [DisplayName("DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DOB { get; set; }
       
        [Required]
        [DisplayName("License Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LicenseExpirationDate { get; set; }

        [Required]
        [DisplayName("DL64 Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DL64ExpirationDate { get; set; }

       
        [DisplayName("DL-64 Number")]      
        public String DL64Number { get; set; }

        [Required]
        [DisplayName("Medical Card Expiration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime MedicalCardExpirationDate { get; set; }

        [DisplayName("Last Pull Notice Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? LastPullNoticeDate { get; set; }

        [Required]
        [DisplayName("Date Added")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }

        [DisplayName("UDF")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public String UDF { get; set; }

        [DisplayName("Comments")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        public String Comments { get; set; }

        [DisplayName("Contractor End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ContractorEndDate { get; set; }

        [DisplayName("Program End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProgramEndDate { get; set; }

        [DisplayName("Contractor Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ContractorStartDate { get; set; }

        [DisplayName("Added to C3 Database")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? AddedtoC3Database { get; set; }

    }
}
