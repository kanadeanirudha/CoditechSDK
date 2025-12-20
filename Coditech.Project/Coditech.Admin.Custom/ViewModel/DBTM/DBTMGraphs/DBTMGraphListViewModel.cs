using Coditech.Common.Helper;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphListViewModel : BaseViewModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int DBTMGraphMasterId { get; set; }
        [Required]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Display(Name = "Graph Mode")]
        public string GraphMode { get; set; }

        public List<SelectListItem> DBTMGraphMasterList { get; set; }

        [Display(Name = "Graph Type")]
        public List<string> DBTMSelectedGraph { get; set; }
    }
}
