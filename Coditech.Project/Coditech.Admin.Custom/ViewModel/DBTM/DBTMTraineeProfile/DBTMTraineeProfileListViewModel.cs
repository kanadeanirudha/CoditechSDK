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
    }
}
