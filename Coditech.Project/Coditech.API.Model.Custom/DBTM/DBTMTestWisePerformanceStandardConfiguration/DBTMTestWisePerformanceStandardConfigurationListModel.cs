namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardConfigurationListModel : BaseListModel
    {
        public List<DBTMTestWisePerformanceStandardConfigurationModel> DBTMTestWisePerformanceStandardConfigurationList { get; set; }
        public DBTMTestWisePerformanceStandardConfigurationListModel()
        {
            DBTMTestWisePerformanceStandardConfigurationList = new List<DBTMTestWisePerformanceStandardConfigurationModel>();
        }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
    }
}
