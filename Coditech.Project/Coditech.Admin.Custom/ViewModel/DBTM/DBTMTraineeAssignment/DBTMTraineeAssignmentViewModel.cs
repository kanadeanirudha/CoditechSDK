using System.ComponentModel.DataAnnotations;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using Coditech.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeAssignmentViewModel : BaseViewModel
    {
        public long DBTMTraineeAssignmentId { get; set; }

        [Required]
        [Display(Name = "Trainee")]
        public long GeneralTrainerMasterId { get; set; }

        [Required]
        public int DBTMTestMasterId { get; set; }

        [Required]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; }

        [Display(Name = "Assignment Time")]
        public TimeSpan? AssignmentTime { get; set; }

        [Required]
        public int DBTMTestStatusEnumId { get; set; }

        [Display(Name = "Test Status")]
        public string TestStatus { get; set; }

        [Required]
        [Display(Name = "LabelCentre", ResourceType = typeof(AdminResources))]
        public string SelectedCentreCode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Test Name")]
        public string TestName { get; set; }
        public string MobileNumber { get; set; }
        public string ImagePath { get; set; }
        public bool IsAssociated { get; set; }
        public List<SelectListItem> AllTraineeList { get; set; }
        [Display(Name = "Trainee")]
        public List<string> SelectedTrainee { get; set; }
        // public bool IsTestActive { get; set; }
    }
}
