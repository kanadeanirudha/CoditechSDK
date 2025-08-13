namespace Coditech.Common.API.Model
{
    public class DBTMYearlyTraineeOverviewModel : BaseModel
    {
        public DBTMYearlyTraineeOverviewModel()
        {
        }
        public string MonthName { get; set; }
        public int TotalTrainee { get; set; }
        public int ActiveTrainee { get; set; }
        public int InactiveTrainee { get; set; }
    }
}
