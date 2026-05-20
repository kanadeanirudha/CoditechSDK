using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeDetailsViewModel : BaseViewModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public long PersonId { get; set; }
        public string PersonCode { get; set; }
        public string CentreCode { get; set; }
        public string UserType { get; set; }
        [MaxLength(500)]
        [Display(Name = "Past Injuries")]
        public string PastInjuries { get; set; }
        [MaxLength(500)]
        [Display(Name = "Medical History")]
        public string MedicalHistory { get; set; }
        [MaxLength(500)]
        [Display(Name = "Other Information")]
        public string OtherInformation { get; set; }
        [Required]
        [Display(Name = "Group")]
        public int? GroupEnumId { get; set; }
        [Required]
        [Display(Name = "Source")]
        public int? SourceEnumId { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public string ImagePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public int NumberOfActivityPerformed { get; set; }
        [Required]
        [RegularExpression(@"^\d{1,3}(\.\d{1,3})?$", ErrorMessage = "Weight must be less than 999")]
        [Display(Name = "Weight(kg)")]
        public decimal Weight { get; set; }
        [Required]
        [RegularExpression(@"^\d{1,3}?$", ErrorMessage = "Height must be less than 999.")]
        [Display(Name = "Height(cm)")]
        public decimal Height { get; set; }
        public long EntityId { get; set; }
        public long GeneralTrainerMasterId { get; set; }
        public string SelectedParameter2 { get; set; }
         [Required]
        [Display(Name = "Specialization")]
        public int? SpecializationEnumId { get; set; }
        public string DisplayName { get; set; }
        [Required]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }
        [Display(Name = "Age Group")]
        public int AgeGroupEnumId { get; set; }
        [Required]
        [Display(Name = "Standard")]
        public string Standard { get; set; }
        [Required]
        [Display(Name = "Section")]
        public string Section { get; set; }
        public string TypeOfCentre { get; set; }
        public string Remarks { get; set; }
        public bool IsBatchUser { get; set; }
        public bool IsCampUser { get; set; }
    }
}
