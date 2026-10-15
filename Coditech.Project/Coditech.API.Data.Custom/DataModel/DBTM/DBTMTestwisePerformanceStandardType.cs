using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestWisePerformanceStandardType
    {
        [Key]
        public short DBTMTestWisePerformanceStandardTypeId { get; set; }
        public string PerformanceStandardType { get; set; }
        public short DefaultPriority { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

