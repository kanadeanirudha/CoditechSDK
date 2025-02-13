namespace Coditech.Common.API.Model
{
    public class DBTMBatchActivityListModel : BaseListModel
    {
        public List<DBTMBatchActivityModel> DBTMBatchActivityList { get; set; }
        public DBTMBatchActivityListModel()
        {
            DBTMBatchActivityList = new List<DBTMBatchActivityModel>();
        }
        public int GeneralBatchMasterId { get; set; }
        public string BatchName { get; set; }
    }
}
