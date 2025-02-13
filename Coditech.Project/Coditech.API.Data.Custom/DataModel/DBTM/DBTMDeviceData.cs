using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMDeviceData
    {
        [Key]
        public long DBTMDeviceDataId { get; set; }
        public string DeviceSerialCode { get; set; }
        public string PersonCode { get; set; }
        public string TestCode { get; set; }
        public string Comments { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

