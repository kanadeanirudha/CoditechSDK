namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfileListModel : BaseListModel
    {
        public List<DBTMTraineeProfileModel> DBTMTraineeProfileList { get; set; }
        public DBTMTraineeProfileListModel()
        {
            DBTMTraineeProfileList = new List<DBTMTraineeProfileModel>();
        }

    }
}
