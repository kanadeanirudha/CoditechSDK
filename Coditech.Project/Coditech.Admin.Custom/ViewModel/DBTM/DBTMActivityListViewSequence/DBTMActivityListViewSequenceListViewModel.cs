using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivityListViewSequenceListViewModel : BaseViewModel
    {
        public List<DBTMActivityListViewSequenceViewModel> DBTMActivityListViewSequenceList { get; set; }
        public DBTMActivityListViewSequenceListViewModel()
        {
            DBTMActivityListViewSequenceList = new List<DBTMActivityListViewSequenceViewModel>();
        }
        public int DBTMTestMasterId { get; set; }
        public string SelectedParameter1 { get; set; }
        public short SequenceNumber { get; set; }
        public int DBTMTestParameterListViewSequenceId { get; set; }
        public string DBTMSequenceData { get; set; }
    }
}
