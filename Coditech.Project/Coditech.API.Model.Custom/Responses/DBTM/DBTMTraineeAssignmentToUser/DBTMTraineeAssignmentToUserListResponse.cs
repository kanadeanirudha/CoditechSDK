namespace Coditech.Common.API.Model.Response
{
    public class DBTMTraineeAssignmentToUserListResponse : BaseListResponse
    {
        public List<DBTMTraineeAssignmentToUserModel> DBTMTraineeAssignmentToUserList { get; set; }
        public string TestName { get; set; }
    }
}
