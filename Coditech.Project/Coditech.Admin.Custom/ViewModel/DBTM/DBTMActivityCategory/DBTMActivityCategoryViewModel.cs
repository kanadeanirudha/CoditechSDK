using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivityCategoryViewModel : BaseViewModel
    {
        public short DBTMActivityCategoryId { get; set; }

        [Display(Name = "Parent Activity Category")]
        public int DBTMParentActivityCategoryId { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Activity Category Code")]
        public string ActivityCategoryCode { get; set; }

        [MaxLength(200)]
        [Required]
        [Display(Name = "Activity Category Name")]
        public string ActivityCategoryName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
}
