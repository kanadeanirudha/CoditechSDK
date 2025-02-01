using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMBatchActivityViewModel : BaseViewModel
    {
        public long DBTMBatchActivityId { get; set; }
        public int GeneralBatchMasterId { get; set; }
        [Required]
        [Display(Name = "Activity")]
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        [Display(Name = "Is Associated")]
        public bool IsAssociated { get; set; }
        public string BatchName { get; set; }
    }
}
