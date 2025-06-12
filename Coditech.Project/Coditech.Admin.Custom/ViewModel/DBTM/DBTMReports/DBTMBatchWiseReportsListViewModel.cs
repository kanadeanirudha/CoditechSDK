using Coditech.Common.Helper;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMBatchWiseReportsListViewModel : BaseViewModel
    {
        public DataTable DataTable { get; set; }
        public DBTMBatchWiseReportsListViewModel()
        {
        }
        public int GeneralBatchMasterId { get; set; }
    }
}
