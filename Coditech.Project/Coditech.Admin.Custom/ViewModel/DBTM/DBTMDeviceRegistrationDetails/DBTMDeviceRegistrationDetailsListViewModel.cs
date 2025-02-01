using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMDeviceRegistrationDetailsListViewModel : BaseViewModel
    {
        public List<DBTMDeviceRegistrationDetailsViewModel> RegistrationDetailsList { get; set; }
        public DBTMDeviceRegistrationDetailsListViewModel()
        {
            RegistrationDetailsList = new List<DBTMDeviceRegistrationDetailsViewModel>();
        }
    }
}
