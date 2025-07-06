using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeAssignmentListViewModel : BaseViewModel
    {
        public List<DBTMTraineeAssignmentViewModel> DBTMTraineeAssignmentList { get; set; }
        public DBTMTraineeAssignmentListViewModel()
        {
            DBTMTraineeAssignmentList = new List<DBTMTraineeAssignmentViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
        public long GeneralTrainerMasterId { get; set; }
    }
}
