namespace Coditech.Common.API.Model
{
    public class DBTMDeviceRegistrationDetailsListModel : BaseListModel
    {
        public List<DBTMDeviceRegistrationDetailsModel> RegistrationDetailsList { get; set; }
        public DBTMDeviceRegistrationDetailsListModel()
        {
            RegistrationDetailsList = new List<DBTMDeviceRegistrationDetailsModel>();
        }
    }
}
