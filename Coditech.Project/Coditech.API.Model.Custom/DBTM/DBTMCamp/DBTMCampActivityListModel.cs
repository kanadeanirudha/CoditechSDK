namespace Coditech.Common.API.Model
{
    public class DBTMCampActivityListModel : BaseListModel
    {
        public List<DBTMCampActivityModel> DBTMCampActivityList { get; set; }
        public DBTMCampActivityListModel()
        {
            DBTMCampActivityList = new List<DBTMCampActivityModel>();
        }
        public long DBTMCampMasterId { get; set; }
        public string CampName { get; set; }
    }
}
