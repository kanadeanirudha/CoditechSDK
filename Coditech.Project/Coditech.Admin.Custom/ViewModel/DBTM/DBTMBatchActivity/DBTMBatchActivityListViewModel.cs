using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMBatchActivityListViewModel : BaseViewModel
    {
        public List<DBTMBatchActivityViewModel> DBTMBatchActivityList { get; set; }
        public DBTMBatchActivityListViewModel()
        {
            DBTMBatchActivityList = new List<DBTMBatchActivityViewModel>();
        }
        public string BatchName { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
    }
}
