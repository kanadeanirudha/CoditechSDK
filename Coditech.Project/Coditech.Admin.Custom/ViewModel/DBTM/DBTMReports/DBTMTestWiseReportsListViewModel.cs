using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWiseReportsListViewModel : BaseViewModel
    {
        public DataTable DataTable { get; set; }
        public DBTMTestWiseReportsListViewModel()
        {
        }
        public int DBTMTestMasterId { get; set; }
        public long DBTMTraineeDetailId { get; set; }
        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
    }
}
