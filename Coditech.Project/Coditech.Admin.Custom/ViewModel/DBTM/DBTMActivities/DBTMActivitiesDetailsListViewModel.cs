using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivitiesDetailsListViewModel : BaseViewModel
    {
        public List<DBTMActivitiesDetailsViewModel> ActivitiesDetailsList { get; set; }
        public List<string> TestColumns { get; set; }
        public List<string> CalculationColumns { get; set; }
        public DBTMActivitiesDetailsListViewModel()
        {
            ActivitiesDetailsList = new List<DBTMActivitiesDetailsViewModel>();
            TestColumns = new List<string>();
            CalculationColumns = new List<string>();
        }
        public string PersonCode { get; set; }
        public long DBTMDeviceDataId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string TestName { get; set; }
    }
}
