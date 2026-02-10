using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;
namespace Coditech.Admin.ViewModel
{
    public class DBTMCentreWiseTestViewModel : BaseViewModel
    {
        public long DBTMCentreWiseTestId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public int OrganisationCentreMasterId { get; set; }
        [Display(Name = "Centre Name")]
        public string CentreCode { get; set; }
        [Display(Name = "Test Name")]
        public string TestName { get; set; }
        public bool IsAssociated { get; set; }
    }
}

