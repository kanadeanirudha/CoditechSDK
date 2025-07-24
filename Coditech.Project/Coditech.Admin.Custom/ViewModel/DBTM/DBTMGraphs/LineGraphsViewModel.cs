namespace Coditech.Admin.ViewModel
{
    public class LineGraphsViewModel
    {
        public string LineChartId { get; set; }
        public string Title { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public string XValues { get; set; }
        public List<LineGraphsDatasetViewModel> Datasets { get; set; }
    }
}

