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
        public int TotalNumberOfActivityPerformedDuringWeek { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }
        public List<DBTMTraineeDetailsModel> Top3Trainee { get; set; }       
    }
}
