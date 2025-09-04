namespace Coditech.Common.API.Model
{
    public class LineChartModel : BaseModel
    {
        public string LineChartId { get; set; }
        public string XValues { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<LineGraphsDatasetModel> Datasets { get; set; }
    }
    public class LineGraphsDatasetModel
    {
        public string Label { get; set; }
        public string Data { get; set; }
        public string Color { get; set; }
    }
}
