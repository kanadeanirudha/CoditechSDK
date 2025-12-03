namespace Coditech.Common.API.Model
{
    public class DBTMNewRegistrationListModel : BaseListModel
    {
        public List<DBTMNewRegistrationModel> DBTMNewRegistrationList { get; set; }
        public DBTMNewRegistrationListModel()
        {
            DBTMNewRegistrationList = new List<DBTMNewRegistrationModel>();
        }
        public string JoiningCode { get; set; }
    }
}
