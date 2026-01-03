using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCentrewiseTestParameterListViewListViewModel : BaseViewModel
    {
        public List<DBTMCentrewiseTestParameterListViewViewModel> DBTMCentrewiseTestParameterListViewList { get; set; }
        public DBTMCentrewiseTestParameterListViewListViewModel()
        {
            DBTMCentrewiseTestParameterListViewList = new List<DBTMCentrewiseTestParameterListViewViewModel>();
        }
    }
}
