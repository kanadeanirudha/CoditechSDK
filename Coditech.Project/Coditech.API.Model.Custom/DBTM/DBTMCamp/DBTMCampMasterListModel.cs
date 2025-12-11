namespace Coditech.Common.API.Model
{
    public class DBTMCampMasterListModel : BaseListModel
    {
        public List<DBTMCampMasterModel> DBTMCampMasterList { get; set; }
        public DBTMCampMasterListModel()
        {
            DBTMCampMasterList = new List<DBTMCampMasterModel>();
        }
    }
}
