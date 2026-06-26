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
        [Display(Name = "Activity Name")]
        public string TestName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Activity Code")]
        public string TestCode { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Graph")]
        public List<string> DBTMSelectedGraph { get; set; }
        public List<SelectListItem> DBTMGraphMasterList { get; set; }
        [Display(Name = "Minimun Paired Device")]
        public short MinimunPairedDevice { get; set; }
        [Display(Name = "Lap Distance")]
        [Required]
        public string LapDistance { get; set; }
        [Display(Name = "Lap Distance Change")]
        public bool IsLapDistanceChange { get; set; }
        [Display(Name = "Multi Test")]
        public bool IsMultiTest { get; set; }
        [Display(Name = "Activity Instructions")]
        public string TestInstructions { get; set; }
        [Required]
        [Display(Name = "Activity Media")]
        public long TestMediaId { get; set; }
        public string TestMediaPath { get; set; }
        public string TestMediaFileName { get; set; }
        public int DBTMGraphMasterId { get; set; }
        [Required]
        [Display(Name = "Performance Matrix")]
        public byte DBTMPerformanceMatrixId { get; set; }
        public string PerformanceMatrix { get; set; }
        [Display(Name = "Start Direction")]
        public bool IsStartDirection { get; set; }
        [Required]
        [Display(Name = "Activity Output Higher")]
        public string TestOutputHigher { get; set; }
        [Display(Name = "Activity Result Basedon")]
        [Required]
        public string TestResultBasedon { get; set; }
    }
}
