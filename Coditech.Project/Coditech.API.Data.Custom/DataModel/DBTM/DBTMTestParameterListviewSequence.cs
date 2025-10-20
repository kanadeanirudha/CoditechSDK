using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestParameterListviewSequence
    {
        [Key]
        public long DBTMTestParameterListviewSequenceId { get; set; }
        public string TestCode { get; set; }
        public string ParameterCode { get; set; }
        public bool IsCalculatedParameter { get; set; }
        public Int16 Recursion { get; set; }
        public Int16 SequenceNumber { get; set; }
        public string ConsecutiveParameterCode { get; set; }
        public bool IsCalculatedConsecutiveParameterCode { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

