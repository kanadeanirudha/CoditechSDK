namespace Coditech.Common.API.Model
{
    public class DBTMTestwisePerformanceStandardCategoryListModel : BaseListModel
    {
        public List<DBTMTestwisePerformanceStandardCategoryModel> DBTMTestwisePerformanceStandardCategoryList { get; set; }
        public DBTMTestwisePerformanceStandardCategoryListModel()
        {
            DBTMTestwisePerformanceStandardCategoryList = new List<DBTMTestwisePerformanceStandardCategoryModel>();
        }
    }
}
