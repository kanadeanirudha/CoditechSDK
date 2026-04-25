using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
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
        public DateTime ToDate { get; set; }
    }
}
