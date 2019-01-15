using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MTC.FSP.Web.Models
{

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EditAccountViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public String UserId { get; set; }

        [StringLength(50), DisplayName("First Name"), Required]
        public String FirstName { get; set; }

        [StringLength(50), DisplayName("Last Name"), Required]
        public String LastName { get; set; }

        [StringLength(50), DisplayName("Nick Name")]
        public String NickName { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(50)]
        public String Mobile { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(50)]
        public String Work { get; set; }

        [DisplayName("Is this User is a contractor, please select associated tow company")]
        public Guid? ContractorId { get; set; }

    }
   
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
       
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(50), DisplayName("First Name"), Required]
        public String FirstName { get; set; }

        [StringLength(50), DisplayName("Last Name"), Required]
        public String LastName { get; set; }

        [StringLength(50), DisplayName("Nick Name")]
        public String NickName { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(50)]
        public String Mobile { get; set; }

        [DataType(DataType.PhoneNumber), StringLength(50)]
        public String Work { get; set; }

        [DisplayName("Is this User is a contractor, please select associated tow company")]
        public Guid? ContractorId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
