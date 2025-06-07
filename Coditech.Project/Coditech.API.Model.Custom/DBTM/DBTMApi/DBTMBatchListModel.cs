namespace Coditech.Common.API.Model
{
    public class DBTMBatchListModel : BaseListModel
    {
        public List<DBTMBatchModel> DBTMBatchList { get; set; }
        public DBTMBatchListModel()
        {
            DBTMBatchList = new List<DBTMBatchModel>();
        }
    }
}
