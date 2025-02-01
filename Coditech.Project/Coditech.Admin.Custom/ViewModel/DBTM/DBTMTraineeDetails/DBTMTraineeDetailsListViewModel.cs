using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeDetailsListViewModel : BaseViewModel
    {
        public List<DBTMTraineeDetailsViewModel> DBTMTraineeDetailsList { get; set; }
        public DBTMTraineeDetailsListViewModel()
        {
            DBTMTraineeDetailsList = new List<DBTMTraineeDetailsViewModel>();
        }
        public string SelectedCentreCode { get; set; } = string.Empty;
        public string ListType { get; set; }
    }
}
