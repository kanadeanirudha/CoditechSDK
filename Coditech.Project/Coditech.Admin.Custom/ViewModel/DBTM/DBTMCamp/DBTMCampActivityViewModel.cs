using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCampActivityViewModel : BaseViewModel
    {
        public long DBTMCampActivityId { get; set; }
        public int DBTMCampMasterId { get; set; }
        [Required]
        [Display(Name = "Activity")]
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        [Display(Name = "Associated")]
        public bool IsAssociated { get; set; }
        public string CampName { get; set; }
    }
}
