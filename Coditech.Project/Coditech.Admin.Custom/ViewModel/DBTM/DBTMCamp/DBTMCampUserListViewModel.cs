using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCampUserListViewModel : BaseViewModel
    {
        public List<DBTMCampUserViewModel> DBTMCampUserList { get; set; }
        public DBTMCampUserListViewModel()
        {
            DBTMCampUserList = new List<DBTMCampUserViewModel>();
        }
        public long DBTMCampMasterId { get; set; }
        public string CampName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
    }
}
