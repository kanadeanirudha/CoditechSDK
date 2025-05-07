using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeAssignmentToUserViewModel : BaseViewModel
    {
        public long DBTMTraineeAssignmentUserId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public long DBTMTraineeAssignmentId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DBTMTestStatusEnumId { get; set; }
        public string MobileNumber { get; set; }
        public string ImagePath { get; set; }
        public bool IsAssociated { get; set; }
        public string TestName { get; set; }
        public bool IsAssignmentActive { get; set; }
    }
}
