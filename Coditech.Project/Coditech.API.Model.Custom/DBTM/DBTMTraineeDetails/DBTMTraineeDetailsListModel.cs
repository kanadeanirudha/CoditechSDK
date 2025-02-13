namespace Coditech.Common.API.Model
{
    public class DBTMTraineeDetailsListModel : BaseListModel
    {
        public List<DBTMTraineeDetailsModel> DBTMTraineeDetailsList { get; set; }
        public DBTMTraineeDetailsListModel()
        {
            DBTMTraineeDetailsList = new List<DBTMTraineeDetailsModel>();
        }

    }
}
