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
        public long GeneralTrainerMasterId { get; set; }
    }
}
