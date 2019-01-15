using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FSP.Domain.Model
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
    }

    public class UserMetaData
    {

        [Required]
        [DisplayName("Role")]   
        public int RoleID { get; set; }


        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(150)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [DisplayName("FirstName")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [DisplayName("Address")]
        [StringLength(100)]
        public string Address { get; set; }

        [DisplayName("City")]
        [StringLength(50)]
        public string City { get; set; }

        [DisplayName("State")]
        [StringLength(2)]
        public string State { get; set; }

        [DisplayName("Zip")]
        [StringLength(10)]
        public string Zip { get; set; }

        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [DisplayName("Wants Notification")]      
        public Boolean WantsNotification { get; set; }

        [DisplayName("Hourly Cost")]
        public float? HourlyCost { get; set; }

        [DisplayName("Approved")]
        public Boolean IsApproved { get; set; }

        [DisplayName("Contractor")]
        public int? ContractorID { get; set; }
    }

}
