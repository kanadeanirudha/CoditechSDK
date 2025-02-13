using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestListViewModel : BaseViewModel
    {
        public List<DBTMTestViewModel> DBTMTestList { get; set; }
        public DBTMTestListViewModel()
        {
            DBTMTestList = new List<DBTMTestViewModel>();
        }
    }
}
