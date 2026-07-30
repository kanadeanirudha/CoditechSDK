using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTestWisePerformanceStandard
    {
        [Key]
        public int DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public decimal ExcellentValue { get; set; }
        public decimal VeryGoodValue { get; set; }
        public decimal GoodValue { get; set; }
        public decimal AverageValue { get; set; }
        public decimal LowValue { get; set; }
        public decimal PoorValue { get; set; }
        public string ExcellentScore { get; set; }
        public string VeryGoodScore { get; set; }
        public string GoodScore { get; set; }
        public string AverageValueScore { get; set; }
        public string LowScore { get; set; }
        public string PoorScore { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

