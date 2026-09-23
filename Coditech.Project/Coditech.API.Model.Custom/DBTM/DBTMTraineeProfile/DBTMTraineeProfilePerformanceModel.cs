namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfilePerformanceModel : BaseModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string TestOutputHigher { get; set; }
        public string PerformanceMatrix { get; set; }
        public string PerformanceMatrixColor { get; set; }
        public bool? IsUp { get; set; }
        public string UpDownValue { get; set; }
        public string Score { get; set; }
        public string BestValue { get; set; }
        public string Unit { get; set; }
    }
}
