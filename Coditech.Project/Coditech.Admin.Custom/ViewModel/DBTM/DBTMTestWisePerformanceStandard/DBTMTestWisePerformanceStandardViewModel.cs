using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardViewModel : BaseViewModel
    {
        public int DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        [Display(Name = "Excellent Value")]
        public short ExcellentValue { get; set; }
        [Display(Name = "Good Value")]
        public short GoodValue { get; set; }
        [Display(Name = "Average Value")]
        public short AverageValue { get; set; }
        public short PoorValue { get; set; }
    }
}
