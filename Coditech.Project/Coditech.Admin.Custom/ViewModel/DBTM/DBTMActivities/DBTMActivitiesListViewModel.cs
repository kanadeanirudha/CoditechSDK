using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivitiesListViewModel : BaseViewModel
    {
        public List<DBTMActivitiesViewModel> ActivitiesList { get; set; }
        public DBTMActivitiesListViewModel()
        {
            ActivitiesList = new List<DBTMActivitiesViewModel>();
        }
        public string PersonCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string CentreCode { get; set; }

    }
}
