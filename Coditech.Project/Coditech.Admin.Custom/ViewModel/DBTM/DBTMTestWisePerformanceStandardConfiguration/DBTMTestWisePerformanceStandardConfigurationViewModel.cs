using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardConfigurationViewModel : BaseViewModel
    {
        public int DBTMTestWisePerformanceStandardConfigurationId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public short DBTMTestwisePerformanceStandardCategoryId { get; set; }
        public short DBTMTestWisePerformanceStandardTypeId { get; set; }
        public short Priority { get; set; }
        public bool IsConfigured { get; set; }
        public string PerformanceStandardType { get; set; }
    }
}
