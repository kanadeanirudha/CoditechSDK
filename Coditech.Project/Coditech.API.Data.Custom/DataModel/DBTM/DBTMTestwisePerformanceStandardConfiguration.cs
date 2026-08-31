using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestWisePerformanceStandardConfiguration
    {
        [Key]
        public int DBTMTestWisePerformanceStandardConfigurationId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public short DBTMTestWisePerformanceStandardTypeId { get; set; }
        public short Priority { get; set; }
        public bool IsConfigured { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

