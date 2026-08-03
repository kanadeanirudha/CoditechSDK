using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTestwisePerformanceStandardCategoryViewModel : BaseViewModel
    {
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Default")]
        public bool IsDefault { get; set; }
    }
}
