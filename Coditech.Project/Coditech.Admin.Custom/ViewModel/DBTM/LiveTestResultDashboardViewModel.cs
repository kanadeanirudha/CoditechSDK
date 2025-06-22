using Coditech.Common.Helper;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class LiveTestResultDashboardViewModel : BaseViewModel
    {
        public List<DataTable> DataTableList { get; set; }
        public LiveTestResultDashboardViewModel()
        {
            DataTableList = new List<DataTable>();
        }
    }
}