using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMApplicationVersionListViewModel : BaseViewModel
    {
        public List<DBTMApplicationVersionViewModel> DBTMApplicationVersionList { get; set; }
        public DBTMApplicationVersionListViewModel()
        {
            DBTMApplicationVersionList = new List<DBTMApplicationVersionViewModel>();
        }
    }
}
