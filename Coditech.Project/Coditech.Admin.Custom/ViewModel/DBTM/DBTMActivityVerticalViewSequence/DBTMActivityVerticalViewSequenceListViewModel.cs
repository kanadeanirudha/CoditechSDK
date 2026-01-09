using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivityVerticalViewSequenceListViewModel : BaseViewModel
    {
        public List<DBTMActivityVerticalViewSequenceViewModel> DBTMActivityVerticalViewSequenceList { get; set; }
        public DBTMActivityVerticalViewSequenceListViewModel()
        {
            DBTMActivityVerticalViewSequenceList = new List<DBTMActivityVerticalViewSequenceViewModel>();
        }
        public int DBTMTestMasterId { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        public string SelectedParameter1 { get; set; }
        public short SequenceNumber { get; set; }
        public int DBTMTestParameterVerticalViewSequenceId { get; set; }
        public string DBTMSequenceData { get; set; }
        public string TestName { get; set; }
        public string DisplayOn { get; set; }
    }
}
