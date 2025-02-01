using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTraineeAssignment
    {
        [Key]
        public long DBTMTraineeAssignmentId { get; set; }
        public long GeneralTrainerMasterId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public TimeSpan? AssignmentTime { get; set; }
        public int DBTMTestStatusEnumId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

