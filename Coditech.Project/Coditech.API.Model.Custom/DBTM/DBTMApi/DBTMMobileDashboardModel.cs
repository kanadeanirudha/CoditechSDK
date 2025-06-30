namespace Coditech.Common.API.Model
{
    public class DBTMMobileDashboardModel 
    {
        public DBTMMobileDashboardModel()
        {
        }
        public int NumberOfTrainers { get; set; }
        public int NumberOfTrainees { get; set; }
        public int TotalNumberOfActivityPerformedDuringWeek { get; set; }
        public List<DBTMTestModel> TopActivityPerformed { get; set; }
        public List<DBTMTraineeAssignmentModel> DueTodayAssignments { get; set; }
    }
}
