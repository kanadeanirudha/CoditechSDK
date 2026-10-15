namespace Coditech.Common.API.Model
{
    public class RadarChartModel : BaseModel
    {
        public string RadarChartId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string GraphType { get; set; } = "radar";
        public string Labels { get; set; }
        public List<RadarGraphsDatasetModel> Datasets { get; set; }
    }
    public class RadarGraphsDatasetModel
    {
        public string Label { get; set; }
        public string Data { get; set; }
        public string Color { get; set; }
        public Dictionary<string, string> TooltipFields { get; set; } = new Dictionary<string, string>();
    }
}
