using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMReportVerticalDataViewModel : BaseViewModel
    {
        public long DBTMDeviceDataId { get; set; }
        public string AthleteName { get; set; }
        public string TestName { get; set; }
        public string Status { get; set; } = "Completed";
        public DateTime TestPerformedTime { get; set; }
        public string Direction { get; set; }
        public List<KeyValuePair<string, DataTable>> DataTableList { get; set; }
        public List<GraphModel> GraphModelList { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public Dictionary<string, string> ActivityDetails { get; set; }
        public Dictionary<string, DataTable> TurnList { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
