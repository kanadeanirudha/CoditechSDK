using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCampActivityViewModel : BaseViewModel
    {
        public long DBTMCampActivityId { get; set; }
        public long DBTMCampMasterId { get; set; }
        [Required]
        [Display(Name = "Activity")]
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        [Display(Name = "Is Associated")]
        public bool IsAssociated { get; set; }
        public string CampName { get; set; }
    }
}
