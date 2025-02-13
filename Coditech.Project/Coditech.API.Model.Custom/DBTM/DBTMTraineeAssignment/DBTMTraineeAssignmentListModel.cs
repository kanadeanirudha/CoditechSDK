namespace Coditech.Common.API.Model
{
    public class DBTMTraineeAssignmentListModel : BaseListModel
    {
        public List<DBTMTraineeAssignmentModel> DBTMTraineeAssignmentList { get; set; }
        public DBTMTraineeAssignmentListModel()
        {
            DBTMTraineeAssignmentList = new List<DBTMTraineeAssignmentModel>();
        }
        public long GeneralTrainerMasterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
