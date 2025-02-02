using Coditech.Resources;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeDetailsCreateEditViewModel : GeneralPersonViewModel
    {
        public DBTMTraineeDetailsCreateEditViewModel()
        {
        }
        public long DBTMTraineeDetailId { get; set; }
        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string SelectedCentreCode { get; set; }
    }
}