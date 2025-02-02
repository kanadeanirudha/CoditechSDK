using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMTraineeAssignmentModel : BaseModel
    {
        public long DBTMTraineeAssignmentId { get; set; }
        [Required]
        public long GeneralTrainerMasterId { get; set; }
        [Required]
        public long DBTMTraineeDetailId { get; set; }
        [Required]
        public int DBTMTestMasterId { get; set; }
        [Required]
        public DateTime AssignmentDate { get; set; }
        public TimeSpan? AssignmentTime { get; set; }
        [Required]
        public int DBTMTestStatusEnumId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TestName { get; set; }
        public string TestStatus { get; set; }
        public string SelectedCentreCode { get; set; }
    }
}
