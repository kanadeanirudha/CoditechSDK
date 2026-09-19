using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMTestWisePerformanceStandardTypeViewModel : BaseViewModel
    {
        public short DBTMTestWisePerformanceStandardTypeId { get; set; }
        public string PerformanceStandardType { get; set; }
        public short DefaultPriority { get; set; }
    }
}
