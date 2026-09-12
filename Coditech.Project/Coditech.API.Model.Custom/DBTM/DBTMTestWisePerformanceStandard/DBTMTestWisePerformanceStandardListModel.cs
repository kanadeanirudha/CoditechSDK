using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardListModel : BaseListModel
    {
        public List<DBTMTestWisePerformanceStandardModel> DBTMTestWisePerformanceStandardList { get; set; }
        public DBTMTestWisePerformanceStandardListModel()
        {
            DBTMTestWisePerformanceStandardList = new List<DBTMTestWisePerformanceStandardModel>();
        }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public DataTable DataTable { get; set; }
        public List<KeyValuePair<string, DataTable>> DataTableList { get; set; }
    }
}
