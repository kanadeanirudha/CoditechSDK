using Coditech.Resources;
using System.Collections.Generic;
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
        public string SelectedParameter2 { get; set; }
        public long GeneralTrainerMasterId { get; set; }
    }
}