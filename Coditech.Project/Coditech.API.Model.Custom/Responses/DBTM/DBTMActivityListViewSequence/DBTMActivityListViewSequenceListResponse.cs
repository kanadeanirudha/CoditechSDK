namespace Coditech.Common.API.Model.Response
{
    public class DBTMActivityListViewSequenceListResponse : BaseListResponse
    {
        public List<DBTMActivityListViewSequenceModel> DBTMActivityListViewSequenceList { get; set; }
        public string TestName { get; set; }
        public string CentreCode { get; set; }
    }
}
