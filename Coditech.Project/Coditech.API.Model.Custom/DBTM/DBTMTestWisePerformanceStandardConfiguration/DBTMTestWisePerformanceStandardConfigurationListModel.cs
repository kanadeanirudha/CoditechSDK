namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardConfigurationListModel : BaseListModel
    {
        public List<DBTMTestWisePerformanceStandardConfigurationModel> DBTMTestWisePerformanceStandardConfigurationList { get; set; }
        public DBTMTestWisePerformanceStandardConfigurationListModel()
        {
            DBTMTestWisePerformanceStandardConfigurationList = new List<DBTMTestWisePerformanceStandardConfigurationModel>();
        }
    }
}
