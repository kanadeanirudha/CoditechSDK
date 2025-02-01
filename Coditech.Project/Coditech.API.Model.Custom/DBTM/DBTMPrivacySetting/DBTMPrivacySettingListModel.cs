namespace Coditech.Common.API.Model
{
    public class DBTMPrivacySettingListModel : BaseListModel
    {
        public List<DBTMPrivacySettingModel> DBTMPrivacySettingList { get; set; }
        public DBTMPrivacySettingListModel()
        {
            DBTMPrivacySettingList = new List<DBTMPrivacySettingModel>();
        }
    }
}
    