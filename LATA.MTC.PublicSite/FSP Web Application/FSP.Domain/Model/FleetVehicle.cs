using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(FleetVehicleMetaData))]
    public partial class FleetVehicle
    {
    }
    public class FleetVehicleMetaData
    {
        [Required]
        [DisplayName("Contractor")]
        public Guid  ContractorID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Program Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProgramStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Program End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ProgramEndDate { get; set; }

        [Required]
        [DisplayName("Fleet Number")]
        [StringLength(50)]
        public String FleetNumber { get; set; }

        [Required]
        [DisplayName("Vehicle Type")]
        [StringLength(50)]
        public String VehicleType { get; set; }

        [Required]
        [DisplayName("Vehicle Year")]
        public int VehicleYear { get; set; }

        [Required]
        [DisplayName("Vehicle Make")]
        [StringLength(50)]
        public String VehicleMake { get; set; }

        [Required]
        [DisplayName("Vehicle Model")]
        [StringLength(50)]
        public String VehicleModel { get; set; }

        [Required]
        [DisplayName("VIN")]
        [StringLength(50)]
        public String VIN { get; set; }

        [Required]
        [DisplayName("License Plate")]
        [StringLength(10)]
        public String LicensePlate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Registration Expiration Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RegistrationExpireDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Insurance Expiration Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime InsuranceExpireDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Last CHP Inspection Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime LastCHPInspection { get; set; }
       
        [DisplayName("Comments")]
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public String Comments { get; set; }

        [Required]
        [DisplayName("Fuel Type")]
        [StringLength(20)]
        public String FuelType { get; set; }

        [Required]
        [DisplayName("Vehicle Number")]
        [StringLength(12)]
        public String VehicleNumber { get; set; }

        [Required]
        [DisplayName("IP Address")]
        [StringLength(20)]
        public String IPAddress { get; set; }

        [Required]
        [DisplayName("FAW")]
        public int FAW { get; set; }

        [Required]
        [DisplayName("RAW")]
        public int RAW { get; set; }

        [Required]
        [DisplayName("RAWR")]
        public int RAWR { get; set; }

        [Required]
        [DisplayName("GVW")]
        public int GVW { get; set; }

        [Required]
        [DisplayName("GVWR")]
        public int GVWR { get; set; }

        [Required]
        [DisplayName("Wheel Base")]
        public int Wheelbase { get; set; }

        [Required]
        [DisplayName("Overhang")]
        public int Overhang { get; set; }

        [Required]
        [DisplayName("MAXTW")]
        public int MAXTW { get; set; }

        [Required]
        [DisplayName("MAXTWCALCDATE")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public String MAXTWCALCDATE { get; set; }

        [DisplayName("Agreement Number")]
        [StringLength(50)]
        public String AgreementNumber { get; set; }

    }
}
