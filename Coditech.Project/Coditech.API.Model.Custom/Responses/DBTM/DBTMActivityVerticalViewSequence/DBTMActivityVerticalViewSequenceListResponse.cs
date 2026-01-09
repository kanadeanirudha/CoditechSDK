namespace Coditech.Common.API.Model.Response
{
    public class DBTMActivityVerticalViewSequenceListResponse : BaseListResponse
    {
        public List<DBTMActivityVerticalViewSequenceModel> DBTMActivityVerticalViewSequenceList { get; set; }
        public string TestName { get; set; }
    }
}
