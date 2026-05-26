using System.Data;

namespace Coditech.Common.API.Model
{
    public class GraphModel : BaseModel
    {
        public LineBarChartModel LineChartModel { get; set; }
        public LineBarChartModel PieChartModel { get; set; }
        public bool IsRecordFound { get; set; }
        public string GraphType { get; set; }
        public string GraphName { get; set; }
        public string GraphSize { get; set; }
        public List<string> DBTMSelectedGraph { get; set; }
        public  DataTable GraphTable { get; set; }
    }
}
