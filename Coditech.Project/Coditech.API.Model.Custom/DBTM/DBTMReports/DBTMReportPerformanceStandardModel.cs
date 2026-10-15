namespace Coditech.Common.API.Model
{
    public class DBTMReportPerformanceStandardModel
    {
        public int GenderEnumId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string PerformanceStandardType { get; set; }
        public int Priority { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double MinScore { get; set; }
        public double MaxScore { get; set; }
    }
}
