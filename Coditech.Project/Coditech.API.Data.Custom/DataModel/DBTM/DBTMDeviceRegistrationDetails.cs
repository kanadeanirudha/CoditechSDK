using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMDeviceRegistrationDetails
    {
        [Key]
        public long DBTMDeviceRegistrationDetailId { get; set; }
        public long DBTMDeviceMasterId { get; set; }
        public string UserType { get; set; }
        public long EntityId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpirationDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

