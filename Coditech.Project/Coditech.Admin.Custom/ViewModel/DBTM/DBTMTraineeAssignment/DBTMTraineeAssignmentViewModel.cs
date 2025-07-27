using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeAssignmentViewModel : BaseViewModel
    {
        public long DBTMTraineeAssignmentId { get; set; }

        [Required]
        public long GeneralTrainerMasterId { get; set; }
        public long EntityId { get; set; }

        [Required]
        public int DBTMTestMasterId { get; set; }

        [Required]
        [Display(Name = "Assignment Date")]
        public DateTime AssignmentDate { get; set; }
        [Required]
        [Display(Name = "Assignment Time")]
        public TimeSpan? AssignmentTime { get; set; }

        [Required]
        public int DBTMTestStatusEnumId { get; set; }

        [Display(Name = "Activity Status")]
        public string TestStatus { get; set; }

        public string SelectedCentreCode { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Activity Name")]
        public string TestName { get; set; }
        public string MobileNumber { get; set; }
        public string ImagePath { get; set; }
        public bool IsAssociated { get; set; }
        public List<SelectListItem> AllTraineeList { get; set; }
        [Display(Name = "Trainee")]
        [Required]
        public List<string> SelectedTrainee { get; set; }
        public DataTable DataTable { get; set; }
        public long DBTMTraineeAssignmentUserId { get; set; }
        public long DBTMDeviceDataId { get; set; }
        public List<SelectListItem> DBTMTestList { get; set; }
        [Display(Name = "Activity")]
        [Required]
        public List<string> SelectedTest { get; set; }

    }
}
