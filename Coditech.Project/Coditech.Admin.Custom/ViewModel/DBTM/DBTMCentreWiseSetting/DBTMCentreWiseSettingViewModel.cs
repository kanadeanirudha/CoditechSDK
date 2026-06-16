using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
namespace Coditech.Admin.ViewModel
{
    public class DBTMCentreWiseSettingViewModel: BaseViewModel
    {
        public long DBTMCentreWiseSettingId { get; set; }
        [Display(Name = "Centre Name")]
        public string CentreCode { get; set; }
        [Required]
        [Display(Name = "Type Of Centre")]
        public string TypeOfCentre { get; set; }
        [Display(Name = "Allow Batch User")]
        [Required]
        public int AllowBatchUser { get; set; }
        [Display(Name = "Allow Camp User")]
        [Required]
        public int AllowCampUser { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        [Display(Name = "Is Display Performance Standard")]
        public bool IsDisplayPerformanceStandard { get; set; }
        public DBTMCentreWiseTestListViewModel TestListViewModel { get; set; }
        public string CentreName { get; set; }
    }
}

