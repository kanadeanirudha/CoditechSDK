namespace Coditech.Common.API.Model
{
    public class DBTMReportsModel : BaseModel
    {
        public long DBTMDeviceDataId { get; set; }
        public string ParameterCode { get; set; }
        public decimal ParameterValue { get; set; }
        public string FromTo { get; set; }
        public Int16 Row { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonName { get; set; }
        public string ActivityStatus { get; set; }
        public DateTime TestPerformedTime { get; set; }
        public int RowCountPerDate { get; set; }
        public int RowOrder { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
    }
}
