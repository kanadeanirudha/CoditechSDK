using Coditech.Common.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphMasterViewModel : BaseViewModel
    {
        public int DBTMGraphMasterId { get; set; }
        [Required]
        [Display(Name = "Graph Name")]
        public string GraphName { get; set; }
        [Required]
        [Display(Name = "Graph Code")]
        public string GraphCode { get; set; }
        [Required]
        [Display(Name = "X Parameter")]
        public string XParameter { get; set; }
        [Required]
        [Display(Name = "X Parameter Based On")]
        public string XParameterBasedOn { get; set; }
        [Required]
        [Display(Name = "Is Y Parameter Calculated")]
        public bool IsYParameterCalculated { get; set; }
        [Required]
        [Display(Name = "Y Parameter")]
        public string YParameter { get; set; }
        [Required]
        [Display(Name = "Y Parameter Based On")]
        public string YParameterBasedOn { get; set; }
        [Required]
        [Display(Name = "X Axis Label")]
        public string XAxixLabel { get; set; }
        [Required]
        [Display(Name = "Y Axis Label")]
        public string YAxixLabel { get; set; }
        [Display(Name = "Activity Name")]
        public string TestCode { get; set; }
        [Required]
        [Display(Name = "Graph Type")]
        public string GraphType { get; set; }
        [Required]
        [Display(Name = "Activity Name")]
        public List<string> DBTMSelectedTestCode { get; set; } = new List<string>();
        public List<SelectListItem> DBTMTestList { get; set; }
        [Display(Name = "Graph Mode")]
        public string GraphMode { get; set; }
        [Required]
        [Display(Name = "Order By")]
        public short OrderBy { get; set; }
        [Required(ErrorMessage = "Graph Size is required")]
        [Display(Name = "Graph Size")]
        public string GraphSize { get; set; }
        public List<string> DBTMSelectedGraph { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Is Calculate Average")]
        public bool IsCalculateAverage { get; set; }
    }
}
