namespace Coditech.Common.API.Model
{
    public class DBTMDeviceListModel : BaseListModel
    {
        public List<DBTMDeviceModel> DBTMDeviceList { get; set; }
        public DBTMDeviceListModel()
        {
            DBTMDeviceList = new List<DBTMDeviceModel>();
        }

    }
}
