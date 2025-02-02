using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestParameterListViewModel : BaseViewModel
    {
        public List<DBTMTestParameterViewModel> DBTMTestParameterList { get; set; }
        public DBTMTestParameterListViewModel()
        {
            DBTMTestParameterList = new List<DBTMTestParameterViewModel>();
        }
    }
}
