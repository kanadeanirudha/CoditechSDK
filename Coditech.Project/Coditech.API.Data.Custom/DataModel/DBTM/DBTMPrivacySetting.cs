using System.ComponentModel.DataAnnotations;
namespace Coditech.API.Data
{
    public partial class DBTMPrivacySetting
    {
        [Key]
        public int DBTMPrivacySettingId { get; set; }
        public string CentreCode { get; set; }
        public bool IsNotificationOn { get; set; }
        public bool IsLocationOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
