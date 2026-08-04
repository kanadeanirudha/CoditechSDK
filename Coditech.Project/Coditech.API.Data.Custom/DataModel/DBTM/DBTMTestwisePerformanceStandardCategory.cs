using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestwisePerformanceStandardCategory
    {
        [Key]
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

