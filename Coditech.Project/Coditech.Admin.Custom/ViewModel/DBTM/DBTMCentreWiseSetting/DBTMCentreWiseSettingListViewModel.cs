using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCentreWiseSettingListViewModel : BaseViewModel
    {
        public List<DBTMCentreWiseSettingViewModel> DBTMCentreWiseSettingList { get; set; }
        public DBTMCentreWiseSettingListViewModel()
        {
            DBTMCentreWiseSettingList = new List<DBTMCentreWiseSettingViewModel>();
        }
    }
}
