using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestWisePerformanceStandard
    {
        [Key]
        public int DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public short ExcellentValue { get; set; }
        public short GoodValue { get; set; }
        public short AverageValue { get; set; }
        public short PoorValue { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

