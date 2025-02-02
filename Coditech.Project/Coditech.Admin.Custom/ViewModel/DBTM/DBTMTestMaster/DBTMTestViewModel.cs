using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestViewModel : BaseViewModel
    {
        public int DBTMTestMasterId { get; set; }

        [Display(Name = "Activity Category")]
        public short DBTMActivityCategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Test Name")]
        public string TestName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Test Code")]
        public string TestCode { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Parameter")]
        public List<string> DBTMSelectedTestParameter { get; set; }
        public List<SelectListItem> DBTMTestParameterList { get; set; }
    }
}
