namespace Coditech.Common.API.Model
{
    public class DBTMTestWisePerformanceStandardConfigurationModel : BaseModel
    {
        public int DBTMTestWisePerformanceStandardConfigurationId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public short DBTMTestWisePerformanceStandardTypeId { get; set; }
        public short Priority { get; set; }
        public bool IsConfigured { get; set; }
    }
}
