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
        [Display(Name = "Y Parameter")]
        public string YParameter { get; set; }
        [Display(Name = "Test Name")]
        public string TestCode { get; set; }
        [Required]
        [Display(Name = "Graph Type")]
        public string GraphType { get; set; }
        [Required]
        [Display(Name = "Test Name")]
        public List<string> DBTMSelectedTestCode { get; set; } = new List<string>();
        public List<SelectListItem> DBTMTestList { get; set; }
        [Display(Name = "Graph Mode")]
        public string GraphMode { get; set; }
    }
}
