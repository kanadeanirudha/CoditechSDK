namespace Coditech.Common.API.Model
{
    public class DBTMCampUserListModel : BaseListModel
    {
        public List<DBTMCampUserModel> DBTMCampUserList { get; set; }
        public DBTMCampUserListModel()
        {
            DBTMCampUserList = new List<DBTMCampUserModel>();
        }
        public string CampName { get; set; }
        public int DBTMCampMasterId { get; set; }
    }
}
