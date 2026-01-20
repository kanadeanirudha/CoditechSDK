using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeUploadResultListViewModel : BaseViewModel
    {
        public List<DBTMTraineeUploadResultViewModel> DBTMTraineeUploadResultList { get; set; }
        public DBTMTraineeUploadResultListViewModel()
        {
            DBTMTraineeUploadResultList = new List<DBTMTraineeUploadResultViewModel>();
        }
    }
}
