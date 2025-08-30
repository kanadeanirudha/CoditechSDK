namespace Coditech.Common.API.Model
{
    public class GraphModel : BaseModel
    {
        public LineChartModel LineChartModel { get; set; }
        public LineChartModel BarChartModel { get; set; }
        public LineChartModel PieChartModel { get; set; }
        public bool IsRecordFound { get; set; }
        public string GraphType { get; set; }
    }
}
