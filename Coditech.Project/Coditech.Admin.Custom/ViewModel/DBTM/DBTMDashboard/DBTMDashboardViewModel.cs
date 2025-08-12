using Coditech.Common.API.Model;
using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMDashboardViewModel : BaseViewModel
    {
        public DBTMDashboardViewModel()
        {
        }       
        public string DBTMDashboardFormEnumCode { get; set; }
        public Int16 NumberOfDaysRecord { get; set; }
        public int NumberOfTrainers { get; set; }
        public int NumberOfTrainees { get; set; }
        public int Assignments { get; set; }
        public int TotalNumberOfActivityPerformedDuringWeek { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }
        public List<DBTMTraineeDetailsModel> Top3Trainee { get; set; }
        public TimeSpentBarGraphsViewModel TimeSpentGraph { get; set; }
        public int NumberOfKits { get; set; }
        public decimal WeeklyDeviceLoad { get; set; }
        public int TotalBatches { get; set; }
        public int TotalTrainees { get; set; }
    }
}