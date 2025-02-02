using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMDeviceMaster
    {
        [Key]
        public long DBTMDeviceMasterId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceSerialCode { get; set; }
        public string Description { get; set; }
        public string ManufacturedBy { get; set; }
        public int StatusEnumId { get; set; }
        public bool IsMasterDevice { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public short WarrantyExpirationPeriodInMonth { get; set; }
        public string AdditionalFeatures { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

