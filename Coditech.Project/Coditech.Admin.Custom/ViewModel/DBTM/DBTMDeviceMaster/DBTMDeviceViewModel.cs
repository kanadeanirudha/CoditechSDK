using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMDeviceViewModel : BaseViewModel
    {
        public long DBTMDeviceMasterId { get; set; }

        [MaxLength(200)]
        [Required]
        [Display(Name = "Device Name")]
        public string DeviceName { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "Serial Code")]
        public string DeviceSerialCode { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(200)]
        [Display(Name = "Manufactured By")]
        public string ManufacturedBy { get; set; }

        [Required]
        public int StatusEnumId { get; set; }
        [Display(Name = "DBTM Device Status")]
        public string DBTMDeviceStatus { get; set; }

        [Display(Name = "Is Master Device")]
        public bool IsMasterDevice { get; set; } = true;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        public DateTime? RegistrationDate { get; set; }

        [Required]
        [Display(Name = "Warranty Expiration Period In Month")]
        public short? WarrantyExpirationPeriodInMonth { get; set; }

        [MaxLength(500)]
        [Display(Name = "Additional Features")]
        public string AdditionalFeatures { get; set; }
    }
}
