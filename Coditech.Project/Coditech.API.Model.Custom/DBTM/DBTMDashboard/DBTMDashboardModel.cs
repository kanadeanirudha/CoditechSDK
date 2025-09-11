namespace Coditech.Common.API.Model
{
    public class DBTMDashboardModel : BaseModel
    {
        public DBTMDashboardModel()
        {
        }
        public string DBTMDashboardFormEnumCode { get; set; }
        public int NumberOfTrainers { get; set; }
        public int NumberOfTrainees { get; set; }
        public int NumberOfActiveTrainees { get; set; }
        public int Assignments { get; set; }
        public string TopAthletes { get; set; }
        public int? TotalNumberOfActivityPerformedDuringWeek { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }
        public List<DBTMTraineeDetailsModel> Top3Trainee { get; set; }
        public int NumberOfKitPurchase { get; set; }
        public int WeeklyDeviceLoad { get; set; }
        public int NumberOfBatches { get; set; }
        public int NumberOfTotalTrainees { get; set; }
        public string TopAthletsName { get; set; }
        public int AdminRoleMasterId { get; set; }
        public long GeneralTrainerMasterId { get; set; }
        public long UserMasterId { get; set; }
        public TimeSpan WeeklyWorkingHours { get; set; }
        public List<DBTMYearlyTraineeOverviewModel> YearlyTraineeOverviewList { get; set; }
        public List<DBTMTrainerDetailsModel> TrainersList { get; set; }
        public DBTMTrainerDetailsModel TrainersDetails { get; set; }
        public List<DBTMCalendarModel> CalendarEvent { get; set; }
    }
}
