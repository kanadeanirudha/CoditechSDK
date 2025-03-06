﻿using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
namespace Coditech.Admin.ViewModel
{
    public class DBTMNewRegistrationViewModel : BaseViewModel
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Centre Name")]
        public string CentreName { get; set; }

        [Required(ErrorMessage = "Joining Code Is Required")]
        [MaxLength(30)]
        [Display(Name = "Centre Code")]
        public string CentreCode { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        [Display(Name = "Title")]
        public string PersonTitle { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int GenderEnumId { get; set; }

        [MaxLength(70)]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailId { get; set; }
        public string EmailIdToken { get; set; }
        public bool IsEmailIdVerifed { get; set; }
        [Required]
        [Display(Name = "City")]
        public int? GeneralCityMasterId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public short GeneralCountryMasterId { get; set; }

        [Required]
        [Display(Name = "Region")]
        public short GeneralRegionMasterId { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Address1")]
        public string AddressLine1 { get; set; }

        [MaxLength(200)]
        [Display(Name = "Address2")]
        public string AddressLine2 { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Pin code")]
        public string Pincode { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Please enter valid Mobile number")]
        [MaxLength(10)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        public string MobileNumberToken { get; set; }
        public bool IsMobileNumberVerifed { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Device Serial Code")]
        public string DeviceSerialCode { get; set; }
        [MaxLength(100)]
        [MinLength(8)]
        [Required(ErrorMessage = "Please Enter The Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&#])[A-Za-z\\d@$!%*?&#]{8,}$",
         ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character (@$!%*?&#).")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [MaxLength(100)]
        [MinLength(8)]
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Terms And Condition")]
        public bool IsTermsAndCondition { get; set; }
        [Required]
        [Display(Name = "Calling Code")]
        public string CallingCode { get; set; }
        [Display(Name = "Specialization")]
        public string TrainerSpecialization { get; set; }
        [Required]
        public int TrainerSpecializationEnumId { get; set; }
        public string UserType { get; set; }
    }
}
