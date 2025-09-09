namespace Coditech.Common.API.Model
{
    public class BarChartModel : BaseModel
    {
        public string BarChartId { get; set; }
        public string XValues { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<BarGraphsDatasetModel> Datasets { get; set; }
    }
    public class BarGraphsDatasetModel
    {
        public string Label { get; set; }
        public string Data { get; set; }
        public string Color { get; set; }
    }
}
