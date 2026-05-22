using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardListViewModel : BaseViewModel
    {
        public List<DBTMTestWisePerformanceStandardViewModel> DBTMTestWisePerformanceStandardList { get; set; }
        public DBTMTestWisePerformanceStandardListViewModel()
        {
            DBTMTestWisePerformanceStandardList = new List<DBTMTestWisePerformanceStandardViewModel>();
        }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
    }
}
