namespace Coditech.Common.API.Model
{
    public class DBTMTraineeAssignmentToUserListModel : BaseListModel
    {
        public List<DBTMTraineeAssignmentToUserModel> DBTMTraineeAssignmentToUserList { get; set; }
        public DBTMTraineeAssignmentToUserListModel()
        {
            DBTMTraineeAssignmentToUserList = new List<DBTMTraineeAssignmentToUserModel>();
        }
    }
}
