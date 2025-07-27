using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphMasterListViewModel : BaseViewModel
    {
        public List<DBTMGraphMasterViewModel> DBTMGraphMasterList { get; set; }
        public DBTMGraphMasterListViewModel()
        {
            DBTMGraphMasterList = new List<DBTMGraphMasterViewModel>();
        }
    }
}
