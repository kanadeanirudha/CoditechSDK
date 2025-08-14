using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Coditech.Common.API.Model
{
    public class DBTMGraphListModel : BaseListModel
    {
        public DBTMReportsListModel dbtmReportsListModel { get; set; }
        public DBTMTestModel dbtmTestModel { get; set; }
        public DataTable DataTable { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        public int DBTMTestMasterId { get; set; }
        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        public bool IsRecordFound { get; set; } = true;
        public string GraphType { get; set; }
        public int DBTMGraphMasterId { get; set; }
        public string LineChartId { get; set; }
        public string Title { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public List<String> XValuesList { get; set; } = new List<String>();
        public List<String> YValuesList { get; set; } = new List<String>();
        public List<string> Colors { get; set; } = new List<string>();
    }
}
