using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMDeviceListViewModel : BaseViewModel
    {
        public List<DBTMDeviceViewModel> DBTMDeviceList { get; set; }
        public DBTMDeviceListViewModel()
        {
            DBTMDeviceList = new List<DBTMDeviceViewModel>();
        }
    }
}
