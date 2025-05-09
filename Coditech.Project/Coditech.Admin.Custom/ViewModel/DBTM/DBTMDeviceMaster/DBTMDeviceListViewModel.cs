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
        public long DBTMDeviceMasterId { get; set; }
        public long DBTMParentDeviceMasterId { get; set; }
        public string SelectedParameter1 { get; set; }
    }
}
