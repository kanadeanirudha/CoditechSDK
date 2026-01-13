using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMOrganisationCentrewiseJoiningCodeListViewModel : BaseViewModel
    {
        public List<DBTMOrganisationCentrewiseJoiningCodeViewModel> DBTMOrganisationCentrewiseJoiningCodeList { get; set; }
        public DBTMOrganisationCentrewiseJoiningCodeListViewModel()
        {
            DBTMOrganisationCentrewiseJoiningCodeList = new List<DBTMOrganisationCentrewiseJoiningCodeViewModel>();
        }
    }
}
