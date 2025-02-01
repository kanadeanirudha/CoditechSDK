using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivitiesDetailsListViewModel : BaseViewModel
    {
        public List<DBTMActivitiesDetailsViewModel> ActivitiesDetailsList { get; set; }
        public List<string> Columns { get; set; }
        public DBTMActivitiesDetailsListViewModel()
        {
            ActivitiesDetailsList = new List<DBTMActivitiesDetailsViewModel>();
            Columns = new List<string>();
        }
        public long DBTMDeviceDataId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonCode { get; set; }
        public string SelectedParameter1 { get; set; }
    }
}
