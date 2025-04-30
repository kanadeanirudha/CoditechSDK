namespace Coditech.Common.API.Model
{
    public class DBTMTraineeAssignmentToUserModel : BaseModel
    {
        public long DBTMTraineeAssignmentUserId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public long DBTMTraineeAssignmentId { get; set; }
        public int DBTMTestStatusEnumId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
