using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfileModel : BaseModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int? SpecializationEnumId { get; set; }
        public long PhotoMediaId { get; set; }
        public string PhotoMediaPath { get; set; }
        public string PhotoMediaFileName { get; set; }
        public string Specialization { get; set; }
        public string PerformanceMatrix { get; set; }
        public string TestName { get; set; }
        public string Score { get; set; }
        public string Rank { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string TotalDuration { get; set; }
        public DateTime? WeekelyHours { get; set; }
        public List<DBTMTraineeProfilePerformanceModel> TraineeProfilePerformanceList { get; set; }
        public string Remarks { get; set; }
        public string CentreCode { get; set; }
        public string TrainerName { get; set; }
        public RadarChartModel RadarChart { get; set; }
        public long GeneralBatchMasterId { get; set; }
    }
}
