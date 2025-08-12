using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMPerformanceMatrixListViewModel : BaseViewModel
    {
        public List<DBTMPerformanceMatrixViewModel> DBTMPerformanceMatrixList { get; set; }
        public DBTMPerformanceMatrixListViewModel()
        {
            DBTMPerformanceMatrixList = new List<DBTMPerformanceMatrixViewModel>();
        }
    }
}
