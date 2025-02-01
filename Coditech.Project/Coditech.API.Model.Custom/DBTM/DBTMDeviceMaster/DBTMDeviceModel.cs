using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMDeviceModel : BaseModel
    {
        public long DBTMDeviceMasterId { get; set; }
        [MaxLength(200)]
        [Required]
        public string DeviceName { get; set; }
        [MaxLength(100)]
        [Required]
        public string DeviceSerialCode { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(200)]
        public string ManufacturedBy { get; set; }
        [Required]
        public int StatusEnumId { get; set; }
        public bool IsMasterDevice { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public DateTime? RegistrationDate { get; set; }
        [Required]
        public short? WarrantyExpirationPeriodInMonth { get; set; }
        [MaxLength(500)]
        public string AdditionalFeatures { get; set; }
        public string DBTMDeviceStatus { get; set; }
    }
}
