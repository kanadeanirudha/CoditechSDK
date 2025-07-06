using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMTraineeAssignmentModel : BaseModel
    {
        public long DBTMTraineeAssignmentId { get; set; }
        public long GeneralTrainerMasterId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public TimeSpan? AssignmentTime { get; set; }
        public int DBTMTestStatusEnumId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TestName { get; set; }
        public string TestStatus { get; set; }
        public string SelectedCentreCode { get; set; }
        public string EmailId { get; set; }
        public string CentreName { get; set; }
        public List<string> SelectedTrainee { get; set; }
        public DataTable DataTable { get; set; }
        public long DBTMTraineeAssignmentUserId { get; set; }
        public long DBTMDeviceDataId { get; set; }

    }
}
