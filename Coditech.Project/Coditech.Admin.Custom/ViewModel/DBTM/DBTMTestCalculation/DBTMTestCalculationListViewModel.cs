using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestCalculationListViewModel : BaseViewModel
    {
        public List<DBTMTestCalculationViewModel> DBTMTestCalculationList { get; set; }
        public DBTMTestCalculationListViewModel()
        {
            DBTMTestCalculationList = new List<DBTMTestCalculationViewModel>();
        }
    }
}
