using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCampMasterViewModel : BaseViewModel
    {
        public long DBTMCampMasterId { get; set; }
        [Required]
        [MaxLength(15)]
        [Display(Name = "Centre Code")]
        public string CentreCode { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Camp Name")]
        public string CampName { get; set; }
        [Required]
        [Display(Name = "Camp Time")]
        public TimeSpan? CampTime { get; set; }
        [Required]
        [Display(Name = "Camp Start Date")]
        public DateTime? CampStartDate { get; set; }
        [Required]
        [Display(Name = "Camp End Date")]
        public DateTime? CampEndDate { get; set; }
    }
}
