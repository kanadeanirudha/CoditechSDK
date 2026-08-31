namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardModel : BaseModel
    {
        public long DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public string GenderDisplayText { get; set; }
        public int DBTMTestWisePerformanceStandardTypeId { get; set; }
        public string PerformanceStandardTypeValue { get; set; }
        public string PerformanceStandardTypeScore { get; set; }
    }
}
