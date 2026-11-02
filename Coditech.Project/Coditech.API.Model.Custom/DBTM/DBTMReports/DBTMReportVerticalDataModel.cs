using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMReportVerticalDataModel : BaseModel
    {
        public long DBTMDeviceDataId { get; set; }
        public string AthleteName { get; set; }
        public string TestName { get; set; }
        public string Direction { get; set; }
        public string Status { get; set; } = "Completed";
        public DateTime TestPerformedTime { get; set; }
        public bool IsValidRecordButton { get; set; }
        public DBTMReportVerticalDataModel()
        {
        }
        public List<GraphModel> GraphModelList = new List<GraphModel>();
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public Dictionary<string, string> ActivityDetails { get; set; }
        public Dictionary<string, DataTable> TurnList { get; set; }
        public string DateOfBirth { get; set; }
    }
}
