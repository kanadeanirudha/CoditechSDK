using Coditech.Common.API.Model;
using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeProfileViewModel : BaseViewModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }
        public decimal Weight { get; set; }
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
        public List<DBTMTraineeProfileModel> TraineeProfiles { get; set; } = new List<DBTMTraineeProfileModel>();

    }
}