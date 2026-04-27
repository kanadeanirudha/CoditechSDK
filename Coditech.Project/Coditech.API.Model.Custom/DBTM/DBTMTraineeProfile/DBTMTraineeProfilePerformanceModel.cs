namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfilePerformanceModel : BaseModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string PerformanceMatrix { get; set; }
        public bool? IsUp { get; set; }
        public string Score { get; set; }
    }
}
