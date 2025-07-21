namespace Coditech.Common.API.Model
{
    public class DBTMGraphMasterListModel : BaseListModel
    {
        public List<DBTMGraphMasterModel> DBTMGraphMasterList { get; set; }
        public DBTMGraphMasterListModel()
        {
            DBTMGraphMasterList = new List<DBTMGraphMasterModel>();
        }

    }
}
