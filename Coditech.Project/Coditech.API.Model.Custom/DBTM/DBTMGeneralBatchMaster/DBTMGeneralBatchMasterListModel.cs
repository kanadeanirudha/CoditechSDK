namespace Coditech.Common.API.Model
{
    public class DBTMGeneralBatchMasterListModel : BaseListModel
    {
        public List<DBTMGeneralBatchMasterModel> DBTMGeneralBatchMasterList { get; set; }
        public DBTMGeneralBatchMasterListModel()
        {
            DBTMGeneralBatchMasterList = new List<DBTMGeneralBatchMasterModel>();
        }
    }
}
