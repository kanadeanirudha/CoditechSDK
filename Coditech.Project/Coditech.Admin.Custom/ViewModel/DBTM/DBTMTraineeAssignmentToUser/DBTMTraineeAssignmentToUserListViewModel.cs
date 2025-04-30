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
    }
}
