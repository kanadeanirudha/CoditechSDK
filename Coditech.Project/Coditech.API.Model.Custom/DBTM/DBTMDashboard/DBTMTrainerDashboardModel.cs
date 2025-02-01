namespace Coditech.Common.API.Model
{
    public class DBTMTrainerDashboardModel : BaseModel
    {
        public int NumberOfTrainees { get; set; }
        public int TotalNumberOfActivityPerformedDuringWeek { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }      
        public List<DBTMTraineeDetailsModel> Top3Trainee { get; set; }
    }
}
