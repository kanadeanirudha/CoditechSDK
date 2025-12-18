namespace Coditech.Common.API.Model
{
    public class LineBarChartModel : BaseModel
    {
        public string LineBarChartId { get; set; }
        public string XValues { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string Title { get; set; } = string.Empty;
        public string GraphType { get; set; }
        public List<LineBarGraphsDatasetModel> Datasets { get; set; }
    }
    public class LineBarGraphsDatasetModel
    {
        public string Label { get; set; }
        public string Data { get; set; }
        public string Color { get; set; }
    }
}
