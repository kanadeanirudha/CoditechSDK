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
        public int NumberOfTraineeTrained { get; set; }
        public int NumberOfBatches { get; set; }
        public int Assignments { get; set; }
        public int? TotalNumberOfActivityPerformedDuringMonth { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }
        public List<DBTMTraineeDetailsModel> Top3Trainee { get; set; }
        public List<UserProfileViewModel> UserProfileModel { get; set; } = new List<UserProfileViewModel>();
        public List<GeneralBatchListViewModel> GeneralBatchList { get; set; } = new List<GeneralBatchListViewModel>();
        public List<DBTMTraineeAssignmentListViewModel> DBTMTraineeAssignmentList { get; set; } = new List<DBTMTraineeAssignmentListViewModel>();
        public TimeSpentBarGraphsViewModel TimeSpentGraph { get; set; }
        public int NumberOfKitPurchase { get; set; }
        public int WeeklyDeviceLoad { get; set; }      
        public int NumberOfActiveTrainees { get; set; }
        public int NumberOfTotalTrainees { get; set; }
        public string TopAthletsName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string SelectedParameter2 { get; set; }
        public TimeSpan WeeklyWorkingHours { get; set; }
        public List<DBTMYearlyTraineeOverviewModel> YearlyTraineeOverviewList { get; set; }
        public List<DBTMTrainerDetailsModel> TrainersList { get; set; }
        public DBTMTrainerDetailsModel TrainersDetails { get; set; }
        public List<DBTMCalendarViewModel> CalendarEvent { get; set; }
    }
}