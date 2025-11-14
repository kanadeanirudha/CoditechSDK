namespace Coditech.Common.API.Model
{
    public class GraphModel : BaseModel
    {
        public LineChartModel LineChartModel { get; set; }
        public BarChartModel BarChartModel { get; set; }
        public LineChartModel PieChartModel { get; set; }
        public bool IsRecordFound { get; set; }
        public string GraphType { get; set; }
        public string GraphName { get; set; }
        public List<string> DBTMSelectedGraph { get; set; }
    }
}
