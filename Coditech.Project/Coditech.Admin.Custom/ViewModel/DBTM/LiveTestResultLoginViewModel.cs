using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class LiveTestResultLoginViewModel : BaseViewModel
    {
        [MaxLength(100)]
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [MaxLength(100)]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }      
        [MaxLength(100)]
        [Required]
        [Display(Name = "Device Serial Code")]
        public string DeviceSerialCode { get; set; }
        [Required]
        [Display(Name = "Activity")]
        public int DBTMTestMasterId { get; set; }     
        [Display(Name = "Activity")]
        public string TestName { get; set; }
    }
}