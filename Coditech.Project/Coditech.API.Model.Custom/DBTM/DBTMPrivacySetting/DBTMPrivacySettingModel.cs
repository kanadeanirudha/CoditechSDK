using System.ComponentModel.DataAnnotations;
namespace Coditech.Common.API.Model
{
    public class DBTMPrivacySettingModel : BaseModel
    {
        public int DBTMPrivacySettingId { get; set; }
        public bool IsNotificationOn { get; set; }
        public string CentreCode { get; set; }
        public bool IsLocationOn { get; set; }
    }
}
