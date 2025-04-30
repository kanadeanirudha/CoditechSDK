using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeAssignmentToUserViewModel : BaseViewModel
    {
        public long DBTMTraineeAssignmentUserId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public long DBTMTraineeAssignmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DBTMTestStatusEnumId { get; set; }
    }
}
