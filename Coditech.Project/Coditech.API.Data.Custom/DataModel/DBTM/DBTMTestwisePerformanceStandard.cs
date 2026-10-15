using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestWisePerformanceStandard
    {
        [Key]
        public long DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestWisePerformanceStandardConfigurationId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public string PerformanceStandardTypeValue { get; set; }
        public string PerformanceStandardTypeScore { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

