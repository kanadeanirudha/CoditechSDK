using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphVerticalViewSequenceListViewModel : BaseViewModel
    {
        public List<DBTMGraphVerticalViewSequenceViewModel> DBTMGraphVerticalViewSequenceList { get; set; }
        public DBTMGraphVerticalViewSequenceListViewModel()
        {
            DBTMGraphVerticalViewSequenceList = new List<DBTMGraphVerticalViewSequenceViewModel>();
        }
        public int DBTMGraphMasterId { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        public string SelectedParameter1 { get; set; }
        public short SequenceNumber { get; set; }
        public int DBTMGraphParameterVerticalViewSequenceId { get; set; }
        public string DBTMSequenceData { get; set; }
        public string GraphName { get; set; }
        public string GraphCode { get; set; }
        public string DisplayOn { get; set; }
    }
}
