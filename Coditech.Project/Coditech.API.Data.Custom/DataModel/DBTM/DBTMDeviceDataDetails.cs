using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMDeviceDataDetails
    {
        [Key]
        public long DBTMDeviceDataDetailId { get; set; }
        public long DBTMDeviceDataId { get; set; }
        public long Time { get; set; }
        public decimal Distance { get; set; }
        public decimal Force { get; set; }
        public decimal Acceleration { get; set; }
        public decimal Angle { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

