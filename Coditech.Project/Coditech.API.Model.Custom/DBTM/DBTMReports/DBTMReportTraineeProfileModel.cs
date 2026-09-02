using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMReportTraineeProfileModel : BaseModel
    {
        public string PhotoMediaPath { get; set; }
        public string BatchName { get; set; }
        public string AssessmentLocation { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int GenderEnumId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AgeGroup { get; set; }
        public int AgeGroupEnumId { get; set; }
        public string AcademyName { get; set; }
        public string TrainerName { get; set; }
        public string Specialization { get; set; }
        public string Rank { get; set; }
        public decimal Weight { get; set; }
        public DateTime? UpdatedWeightDate { get; set; }
        public decimal Height { get; set; }
        public DateTime? UpdatedHeightDate { get; set; }
        public decimal BMI { get; set; }
        public string CentreName { get; set; }
        public RadarChartModel RadarChart { get; set; }
        public bool IsListView { get; set; } = false;
        public List<DBTMTraineeProfilePerformanceModel> TraineeProfilePerformanceList { get; set; }
        public string SelectedCentreCode { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
        public string Remarks { get; set; }
        public string SchoolName { get; set; }
        public DateTime AssessmentDate { get; set; }
        //public string PerformanceMatrix { get; set; }
        //public string TestName { get; set; }
        //public string Score { get; set; }
        //public DateTime? DateOfJoining { get; set; }
        //public string TotalDuration { get; set; }
        //public DateTime? WeekelyHours { get; set; }
        //public string Remarks { get; set; }
        //public string CentreCode { get; set; }
        //public DataTable RankingScoreDataTable { get; set; }
        //public long GeneralBatchMasterId { get; set; }
        //public string Session { get; set; }
        //public int TotalParticipants { get; set; }
    }
}
