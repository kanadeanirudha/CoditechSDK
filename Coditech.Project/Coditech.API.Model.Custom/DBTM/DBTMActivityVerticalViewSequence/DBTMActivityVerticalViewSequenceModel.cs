namespace Coditech.Common.API.Model
{
    public class DBTMActivityVerticalViewSequenceModel : BaseModel
    {
        public int DBTMTestParameterVerticalViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string ParameterCode { get; set; }
        public bool IsCalculatedParameter { get; set; }
        public short Recursion { get; set; }
        public short SequenceNumber { get; set; }
        public string ConsecutiveParameterCode { get; set; }
        public bool? IsCalculatedConsecutiveParameterCode { get; set; }
        public string ColumnName { get; set; }
        public string ColumnDisplayName { get; set; }
        public string HelpText { get; set; }
        public List<DBTMActivityVerticalViewSequenceModel> DBTMActivityVerticalViewSequenceList { get; set; }
        public string DisplayOn { get; set; }
        public string ColumnCellColor { get; set; }
        public bool IsColumnCellBold { get; set; }
        public string TestName { get; set; }
        public string StaticValue { get; set; }
    }
}
