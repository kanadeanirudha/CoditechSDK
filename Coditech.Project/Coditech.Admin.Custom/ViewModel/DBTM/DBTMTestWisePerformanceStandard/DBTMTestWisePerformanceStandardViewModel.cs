using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardViewModel : BaseViewModel
    {
        public long DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public int DBTMTestWisePerformanceStandardTypeId { get; set; }
        public string PerformanceStandardTypeValue { get; set; }
        public string PerformanceStandardTypeScore { get; set; }
        public string GenderDisplayText { get; set; }
        [Display(Name = "Excellent Value")]
        public decimal ExcellentValue { get; set; }
        [Display(Name = "Very Good Value")]
        public decimal VeryGoodValue { get; set; }
        [Display(Name = "Good Value")]
        public decimal GoodValue { get; set; }
        [Display(Name = "Average Value")]
        public decimal AverageValue { get; set; }
        public decimal LowValue { get; set; }
        public decimal PoorValue { get; set; }
        public string AgeGroupDisplayText { get; set; }
        public string ExcellentScore { get; set; }
        public string VeryGoodScore { get; set; }
        public string GoodScore { get; set; }
        public string AverageValueScore { get; set; }
        public string LowScore { get; set; }
        public string PoorScore { get; set; }
    }
}
