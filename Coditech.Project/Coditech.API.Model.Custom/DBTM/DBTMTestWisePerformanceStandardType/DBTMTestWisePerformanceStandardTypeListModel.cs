namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardTypeListModel : BaseListModel
    {
        public List<DBTMTestWisePerformanceStandardTypeModel> DBTMTestWisePerformanceStandardTypeList { get; set; }
        public DBTMTestWisePerformanceStandardTypeListModel()
        {
            DBTMTestWisePerformanceStandardTypeList = new List<DBTMTestWisePerformanceStandardTypeModel>();
        }
    }
}
