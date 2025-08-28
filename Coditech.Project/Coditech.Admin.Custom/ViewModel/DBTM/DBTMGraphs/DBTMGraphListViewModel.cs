using Coditech.Common.Helper;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphListViewModel : BaseViewModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int DBTMGraphMasterId { get; set; }
        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
    }
}
