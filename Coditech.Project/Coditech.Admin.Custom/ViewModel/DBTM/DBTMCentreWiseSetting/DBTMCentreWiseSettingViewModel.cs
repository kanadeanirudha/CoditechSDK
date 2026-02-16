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
        [Display(Name = "Allow Joining Code Quantity")]
        [Required]
        public int AllowJoiningCode { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        public DBTMCentreWiseTestListViewModel TestListViewModel { get; set; }
    }
}

