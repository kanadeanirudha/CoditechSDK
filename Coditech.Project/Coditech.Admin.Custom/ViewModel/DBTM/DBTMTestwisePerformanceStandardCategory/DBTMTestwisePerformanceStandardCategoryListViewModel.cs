using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTestwisePerformanceStandardCategoryListViewModel : BaseViewModel
    {
        public List<DBTMTestwisePerformanceStandardCategoryViewModel> DBTMTestwisePerformanceStandardCategoryList { get; set; }
        public DBTMTestwisePerformanceStandardCategoryListViewModel()
        {
            DBTMTestwisePerformanceStandardCategoryList = new List<DBTMTestwisePerformanceStandardCategoryViewModel>();
        }
    }
}
