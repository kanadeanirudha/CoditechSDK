namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfilePerformanceRankingScoreModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public string Name { get; set; }
        public Dictionary<string, decimal> TestResult { get; set; }
        public int Rank { get; set; }
    }
}
