using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestParameterVerticalViewSequence
    {
        [Key]
        public int DBTMTestParameterVerticalViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string ParameterCode { get; set; }
        public bool IsCalculatedParameter { get; set; }
        public Int16 Recursion { get; set; }
        public Int16 SequenceNumber { get; set; }
        public string ConsecutiveParameterCode { get; set; }
        public bool? IsCalculatedConsecutiveParameterCode { get; set; }
        public string? ColumnName { get; set; }
        public string? ColumnDisplayName { get; set; }
        public string HelpText { get; set; }
        public string DisplayOn { get; set; }
        public string ColumnCellColor { get; set; }
        public bool? IsColumnCellBold { get; set; }
        public string StaticValue { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

