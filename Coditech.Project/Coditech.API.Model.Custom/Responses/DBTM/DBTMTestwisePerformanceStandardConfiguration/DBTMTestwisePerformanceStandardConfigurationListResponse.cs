namespace Coditech.Common.API.Model.Response
{
    public class DBTMTestWisePerformanceStandardConfigurationListResponse : BaseListResponse
    {
        public List<DBTMTestWisePerformanceStandardConfigurationModel> DBTMTestWisePerformanceStandardConfigurationList { get; set; }
        public string TestName { get; set; }
    }
}
