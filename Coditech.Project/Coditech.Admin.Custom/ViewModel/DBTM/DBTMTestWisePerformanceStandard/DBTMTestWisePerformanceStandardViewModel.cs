using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardViewModel : BaseViewModel
    {
        public long DBTMTestWisePerformanceStandardId { get; set; }
        public int DBTMTestWisePerformanceStandardConfigurationId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public int AgeGroupEnumId { get; set; }
        public int GenderEnumId { get; set; }
        public string PerformanceStandardTypeValue { get; set; }
        public string PerformanceStandardTypeScore { get; set; }
        public string AgeGroupDisplayText { get; set; }
        public string GenderDisplayText { get; set; }
        public int DBTMTestMasterId { get; set; }
        public List<DBTMTestWisePerformanceStandardViewModel> PerformanceStandards { get; set; }
    }
}
