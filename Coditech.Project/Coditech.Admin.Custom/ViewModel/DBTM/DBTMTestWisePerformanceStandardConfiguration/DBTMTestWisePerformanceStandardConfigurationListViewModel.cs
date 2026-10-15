using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardConfigurationListViewModel : BaseViewModel
    {
        public List<DBTMTestWisePerformanceStandardConfigurationViewModel> DBTMTestWisePerformanceStandardConfigurationList { get; set; }
        public DBTMTestWisePerformanceStandardConfigurationListViewModel()
        {
            DBTMTestWisePerformanceStandardConfigurationList = new List<DBTMTestWisePerformanceStandardConfigurationViewModel>();
        }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
    }
}
