using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMNewRegistrationListViewModel : BaseViewModel
    {
        public List<DBTMNewRegistrationViewModel> DBTMNewRegistrationList { get; set; }
        public DBTMNewRegistrationListViewModel()
        {
            DBTMNewRegistrationList = new List<DBTMNewRegistrationViewModel>();
        }
    }
}
