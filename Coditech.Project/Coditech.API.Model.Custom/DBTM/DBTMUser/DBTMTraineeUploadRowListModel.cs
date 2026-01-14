namespace Coditech.Common.API.Model
{
    public class DBTMTraineeUploadRowListModel : BaseListModel
    {
        public List<DBTMTraineeUploadRowModel> DBTMTraineeUploadRowList { get; set; }
        public DBTMTraineeUploadRowListModel()
        {
            DBTMTraineeUploadRowList = new List<DBTMTraineeUploadRowModel>();
        }

    }
}
