using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMPrivacySettingListViewModel : BaseViewModel
    {
        public List<DBTMPrivacySettingViewModel> DBTMPrivacySettingList { get; set; }
        public DBTMPrivacySettingListViewModel()
        {
            DBTMPrivacySettingList = new List<DBTMPrivacySettingViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
    }
}
