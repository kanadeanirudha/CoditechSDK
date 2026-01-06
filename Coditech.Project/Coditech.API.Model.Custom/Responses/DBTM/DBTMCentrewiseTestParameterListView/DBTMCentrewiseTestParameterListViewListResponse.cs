namespace Coditech.Common.API.Model.Response
{
    public class DBTMCentrewiseTestParameterListViewListResponse : BaseListResponse
    {
        public List<DBTMCentrewiseTestParameterListViewModel> DBTMCentrewiseTestParameterListViewList { get; set; }
        public string TestName { get; set; }
    }
}
