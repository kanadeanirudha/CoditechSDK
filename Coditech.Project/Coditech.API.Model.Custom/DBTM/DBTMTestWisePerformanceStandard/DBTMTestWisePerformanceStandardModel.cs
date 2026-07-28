namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardModel : BaseModel
    {
        public int DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public string AgeGroupDisplayText { get; set; }
        public int GenderEnumId { get; set; }
        public string GenderDisplayText { get; set; }
        public decimal ExcellentValue { get; set; }
        public decimal VeryGoodValue { get; set; }
        public decimal GoodValue { get; set; }
        public decimal AverageValue { get; set; }
        public decimal LowValue { get; set; }
        public decimal PoorValue { get; set; }
        public string ExcellentScore { get; set; }
        public string VeryGoodScore { get; set; }
        public string GoodScore { get; set; }
        public string AverageScore { get; set; }
        public string LowScore { get; set; }
        public string PoorScore { get; set; }
    }
}
