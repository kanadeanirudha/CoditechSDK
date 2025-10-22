namespace Coditech.Common.API.Model
{
    public class DBTMActivityListViewSequenceModel : BaseModel
    {
        public int DBTMTestParameterListViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string ParameterCode { get; set; }
        public bool IsCalculatedParameter { get; set; }
        public short Recursion { get; set; }
        public short SequenceNumber { get; set; }
        public string ConsecutiveParameterCode { get; set; }
        public bool IsCalculatedConsecutiveParameterCode { get; set; }
        public string ColumnName { get; set; }
    }
}
