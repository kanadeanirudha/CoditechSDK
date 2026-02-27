using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCampActivityListViewModel : BaseViewModel
    {
        public List<DBTMCampActivityViewModel> DBTMCampActivityList { get; set; }
        public DBTMCampActivityListViewModel()
        {
            DBTMCampActivityList = new List<DBTMCampActivityViewModel>();
        }
        public string CampName { get; set; }
        public int DBTMCampMasterId { get; set; }
    }
}
