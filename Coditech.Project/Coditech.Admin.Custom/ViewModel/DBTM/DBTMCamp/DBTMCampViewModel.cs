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
        [Display(Name = "Frequency")]
        public string CampFrequency { get; set; }
        public string WeekDays { get; set; }
        [Display(Name = "Duration")]
        public TimeSpan? Duration { get; set; }
        [Display(Name = "Weekly")]
        public List<string> SelectedWeekDays { get; set; } = new List<string>();
        public List<SelectListItem> SchedulerWeekDaysList { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Duration Hours is required.")]
        public string DurationHours { get; set; }
        [Required(ErrorMessage = "Duration Minutes is required.")]
        public string DurationMinutes { get; set; }
    }
}
