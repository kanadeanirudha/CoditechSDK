using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardTypeListViewModel : BaseViewModel
    {
        public List<DBTMTestWisePerformanceStandardTypeViewModel> DBTMTestWisePerformanceStandardTypeList { get; set; }
        public DBTMTestWisePerformanceStandardTypeListViewModel()
        {
            DBTMTestWisePerformanceStandardTypeList = new List<DBTMTestWisePerformanceStandardTypeViewModel>();
        }
    }
}
