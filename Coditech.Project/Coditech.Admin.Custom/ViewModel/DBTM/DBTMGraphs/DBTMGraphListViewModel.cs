using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphListViewModel : BaseViewModel
    {
        public DataTable DataTable { get; set; }
        public DBTMGraphListViewModel()
        {
        }
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
    }
}
