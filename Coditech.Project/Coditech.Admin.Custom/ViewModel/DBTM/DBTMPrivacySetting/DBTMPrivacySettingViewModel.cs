using Coditech.Common.Helper;
using Coditech.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMPrivacySettingViewModel : BaseViewModel
    {
        public int DBTMPrivacySettingId { get; set; }

        [Display(Name = "Is Notification On")]
        public bool IsNotificationOn { get; set; }

        [Display(Name = "Is Location On")]
        public bool IsLocationOn { get; set; }
        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string CentreCode { get; set; }

    }
}
