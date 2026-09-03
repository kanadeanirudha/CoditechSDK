namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardModel : BaseModel
    {
        public long DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestWisePerformanceStandardConfigurationId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public string GenderDisplayText { get; set; }
        public string PerformanceStandardTypeValue { get; set; }
        public string PerformanceStandardTypeScore { get; set; }
    }
}
