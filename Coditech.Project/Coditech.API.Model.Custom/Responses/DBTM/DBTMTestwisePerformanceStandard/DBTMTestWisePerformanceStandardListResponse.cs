namespace Coditech.Common.API.Model.Response
{
    public class DBTMTestWisePerformanceStandardListResponse : BaseListResponse
    {
        public List<DBTMTestWisePerformanceStandardModel> DBTMTestWisePerformanceStandardList { get; set; }
        public string TestName { get; set; }
    }
}
