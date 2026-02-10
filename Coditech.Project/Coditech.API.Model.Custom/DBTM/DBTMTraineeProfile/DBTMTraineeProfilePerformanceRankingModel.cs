namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfilePerformanceRankingModel : BaseModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public string Name { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public decimal? BestTime { get; set; }
        public decimal? BestLength { get; set; }
        public decimal? BestHeight { get; set; }
        public decimal? BestCount { get; set; }
        public string TestResultBasedon { get; set; }
    }
}
