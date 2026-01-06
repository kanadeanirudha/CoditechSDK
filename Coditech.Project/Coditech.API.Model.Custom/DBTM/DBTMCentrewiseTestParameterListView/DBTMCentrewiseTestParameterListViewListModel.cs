namespace Coditech.Common.API.Model
{
    public class DBTMCentrewiseTestParameterListViewListModel : BaseListModel
    {
        public List<DBTMCentrewiseTestParameterListViewModel> DBTMCentrewiseTestParameterListViewList { get; set; }
        public DBTMCentrewiseTestParameterListViewListModel()
        {
            DBTMCentrewiseTestParameterListViewList = new List<DBTMCentrewiseTestParameterListViewModel>();
        }
    }
}
