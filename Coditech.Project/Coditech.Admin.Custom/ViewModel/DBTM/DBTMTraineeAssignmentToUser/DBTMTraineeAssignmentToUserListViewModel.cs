using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeAssignmentToUserListViewModel : BaseViewModel
    {
        public List<DBTMTraineeAssignmentToUserViewModel> DBTMTraineeAssignmentToUserList { get; set; }
        public DBTMTraineeAssignmentToUserListViewModel()
        {
            DBTMTraineeAssignmentToUserList = new List<DBTMTraineeAssignmentToUserViewModel>();
        }
        public long DBTMTraineeAssignmentId { get; set; }
        public string TestName { get; set; }
        public string SelectedParameter1 { get; set; }
    }
}
