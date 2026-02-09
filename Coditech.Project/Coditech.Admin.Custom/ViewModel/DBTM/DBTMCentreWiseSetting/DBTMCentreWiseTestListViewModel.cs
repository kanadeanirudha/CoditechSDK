using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMCentreWiseTestListViewModel : BaseViewModel
    {
        public List<DBTMCentreWiseTestViewModel> DBTMCentreWiseTestList { get; set; }
        public DBTMCentreWiseTestListViewModel()
        {
            DBTMCentreWiseTestList = new List<DBTMCentreWiseTestViewModel>();
        }
    }
}
