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
        public int Assignments { get; set; }
        public string TopAthletes { get; set; }
        public int? TotalNumberOfActivityPerformedDuringMonth { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }
        public List<DBTMTraineeDetailsModel> Top3Trainee { get; set; }
        public int NumberOfKitPurchase { get; set; }
        public int WeeklyDeviceLoad { get; set; }
        public int NumberOfBatches { get; set; }
        public int TotalTrainees { get; set; }
        public string TopAthletsName { get; set; }
        public string EmployeeDesignation { get; set; }
        public string TrainerSpecialization { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DurationWithUs { get; set; }
        public int NumberOfBatchesCreated { get; set; }
        public TimeSpan WeeklyWorkingHours { get; set; }
        public List<DBTMYearlyTraineeOverviewModel> YearlyTraineeOverviewList { get; set; }
        public List<DBTMTrainerDetailsModel> TrainersList { get; set; }
        public DBTMTrainerDetailsModel TrainersDetails { get; set; }
    }
}
