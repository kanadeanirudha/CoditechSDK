namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardListModel : BaseListModel
    {
        public List<DBTMTestWisePerformanceStandardModel> DBTMTestWisePerformanceStandardList { get; set; }
        public DBTMTestWisePerformanceStandardListModel()
        {
            DBTMTestWisePerformanceStandardList = new List<DBTMTestWisePerformanceStandardModel>();
        }
    }
}
