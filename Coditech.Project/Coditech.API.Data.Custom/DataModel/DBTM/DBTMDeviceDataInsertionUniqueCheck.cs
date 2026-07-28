using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMDeviceDataInsertionUniqueCheck
    {
        [Key]
        public string DataUniqueId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}

