using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMApplicationVersionViewModel : BaseViewModel
    { 
        public long DBTMApplicationVersionId { get; set; }

        [Required]
        [Display(Name = "Application Type")]
        public string ApplicationType { get; set; }

        [Required]
        [Display(Name = "Version")]
        public string Version { get; set; }

        [Required]
        [Display(Name = "Version Details")]
        public string VersionDetails { get; set; }

        [Required]
        [Display(Name = "URL")]
        public string URL { get; set; }

        [Display(Name = "Latest Version")]
        public bool IsLatestVersion { get; set; }
    }
}
