namespace Coditech.Common.API.Model.Response
{
    public class DBTMGraphVerticalViewSequenceListResponse : BaseListResponse
    {
        public List<DBTMGraphVerticalViewSequenceModel> DBTMGraphVerticalViewSequenceList { get; set; }
        public string GraphName { get; set; }
        public string GraphCode { get; set; }
    }
}
