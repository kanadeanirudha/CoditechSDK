namespace Coditech.Common.API.Model
{
    public class DBTMCentreWiseSettingListModel : BaseListModel
    {
        public List<DBTMCentreWiseSettingModel> DBTMCentreWiseSettingList { get; set; }
        public DBTMCentreWiseSettingListModel()
        {
            DBTMCentreWiseSettingList = new List<DBTMCentreWiseSettingModel>();
        }
    }
}
