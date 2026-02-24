using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeProfileListViewModel : BaseViewModel
    {
        public List<DBTMTraineeProfileViewModel> DBTMTraineeProfileList { get; set; }
        public DBTMTraineeProfileListViewModel()
        {
            DBTMTraineeProfileList = new List<DBTMTraineeProfileViewModel>();
        }
        public long DBTMTraineeDetailId { get; set; }
        public long GeneralBatchMasterId { get; set; }
        public string OrderBy { get; set; }
    }
}
