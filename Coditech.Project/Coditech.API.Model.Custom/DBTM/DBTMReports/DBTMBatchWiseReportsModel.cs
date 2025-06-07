namespace Coditech.Common.API.Model
{
    public class DBTMBatchWiseReportsModel : BaseModel
    {
        public string ParameterCode { get; set; }
        public decimal ParameterValue { get; set; }
        public string FromTo { get; set; }
        public string Row { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string TestName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? TestPerformedTime { get; set; }
    }
}
