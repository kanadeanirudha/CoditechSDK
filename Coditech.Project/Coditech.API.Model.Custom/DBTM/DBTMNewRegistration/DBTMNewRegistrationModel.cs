using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMNewRegistrationModel : BaseModel
    {
        public long DBTMNewRegistrationId { get; set; }
        [MaxLength(30)]
        [Display(Name = "Centre Code")]
        public string CentreCode { get; set; }
        [MaxLength(100)]
        [Display(Name = "Centre Name")]
        public string CentreName { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(70)]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "City")]
        public int GeneralCityMasterId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public short GeneralCountryMasterId { get; set; }

        [Required]
        [Display(Name = "Region")]
        public short GeneralRegionMasterId { get; set; }

        [MaxLength(200)]
        [Required]
        [Display(Name = "Address1")]
        public string AddressLine1 { get; set; }

        [MaxLength(200)]
        [Display(Name = "Address2")]
        public string AddressLine2 { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Pin code")]
        public string Pincode { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }
        public string DeviceSerialCode { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public bool TermsAndCondition { get; set; }
        [Required]
        public string PersonTitle { get; set; }
        [Required]
        public int GenderEnumId { get; set; }
        [Required]
        public string CallingCode { get; set; }
        public int TrainerSpecializationEnumId { get; set; }
    }
}
