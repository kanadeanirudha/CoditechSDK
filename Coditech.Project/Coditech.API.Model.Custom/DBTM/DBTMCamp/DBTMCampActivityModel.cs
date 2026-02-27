namespace Coditech.Common.API.Model
{
    public class DBTMCampActivityModel : BaseModel
    {
        public long DBTMCampActivityId { get; set; }
        public int DBTMCampMasterId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public bool IsAssociated { get; set; }
        public string CampName { get; set; }
        public string PerformanceMatrix { get; set; }
    }
}
