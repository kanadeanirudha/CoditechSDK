using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTraineeAssignmentToUser
    {
        [Key]
        public long DBTMTraineeAssignmentUserId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public long DBTMTraineeAssignmentId { get; set; }
        public int DBTMTestStatusEnumId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

