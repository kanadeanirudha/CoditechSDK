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

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Parameter")]
        public List<string> DBTMSelectedTestParameter { get; set; }
        [Required]
        [Display(Name = "Graph")]
        public List<string> DBTMSelectedGraph { get; set; }
        [Required]
        [Display(Name = "Calculation")]
        public List<string> DBTMSelectedTestCalculation { get; set; }
        public List<SelectListItem> DBTMTestParameterList { get; set; }
        public List<SelectListItem> DBTMGraphMasterList { get; set; }
        public List<SelectListItem> DBTMTestCalculationList { get; set; }
        [Display(Name = "Minimun Paired Device")]
        public short MinimunPairedDevice { get; set; }
        [Display(Name = "Lap Distance")]
        [Required]
        public string LapDistance { get; set; }
        [Display(Name = "Is Lap Distance Change")]
        public bool IsLapDistanceChange { get; set; }
        [Display(Name = "Is Multi Test")]
        public bool IsMultiTest { get; set; }
        [Display(Name = "Activity Instructions")]
        public string TestInstructions { get; set; }
        [Required]
        [Display(Name = "Activity Media")]
        public long TestMediaId { get; set; }
        public string TestMediaPath { get; set; }
        public string TestMediaFileName { get; set; }
        public int DBTMGraphMasterId { get; set; }
    }
}
