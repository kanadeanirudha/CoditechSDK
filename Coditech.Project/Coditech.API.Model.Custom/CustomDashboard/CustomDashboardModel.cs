namespace Coditech.Common.API.Model
{
    public class CustomDashboardModel : BaseModel
    {
        public CustomDashboardModel()
        {
        }
        public string DBTMDashboardFormEnumCode { get; set; }
        public int NumberOfTrainers { get; set; }
        public int NumberOfTrainees { get; set; }
        public int Assignments { get; set; }        
        public int TotalNumberOfActivityPerformedDuringWeek { get; set; }
    }
}
