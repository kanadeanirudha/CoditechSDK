namespace Coditech.Common.API.Model
{
    public class DBTMTraineeProfilePerformanceModel : BaseModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public string TestCode { get; set; }
        public string TestName { get; set; }
        public string PerformanceMatrix { get; set; }
        //public string ParameterCode { get; set; }
        //public string ParameterValue { get; set; }
        //public int RowNumber { get; set; }
        public bool? IsUp { get; set; }
        public string Score { get; set; }
    }
}
