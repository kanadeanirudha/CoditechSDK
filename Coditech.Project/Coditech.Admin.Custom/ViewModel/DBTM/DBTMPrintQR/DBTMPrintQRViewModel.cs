using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMPrintQRViewModel : BaseViewModel
    {
        public long PersonId { get; set; }
        public int GeneralBatchMasterId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string PersonCode { get; set; }
        public string QRCode { get; set; }
        public string PrintableHTML { get; set; }
    }
}
