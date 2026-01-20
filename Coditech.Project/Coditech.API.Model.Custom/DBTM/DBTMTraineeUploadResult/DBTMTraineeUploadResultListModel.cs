namespace Coditech.Common.API.Model
{
    public class DBTMTraineeUploadResultListModel : BaseListModel
    {
        public List<DBTMTraineeUploadResultModel> DBTMTraineeUploadResultList { get; set; }
        public DBTMTraineeUploadResultListModel()
        {
            DBTMTraineeUploadResultList = new List<DBTMTraineeUploadResultModel>();
        }

    }
}
