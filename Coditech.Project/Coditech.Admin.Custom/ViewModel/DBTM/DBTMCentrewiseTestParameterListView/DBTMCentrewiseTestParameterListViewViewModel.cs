using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMCentrewiseTestParameterListViewViewModel : BaseViewModel
    {
        public long DBTMCentrewiseTestParameterListViewId { get; set; }
        public int DBTMTestParameterListViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }

        [Display(Name = "Parameter Code")]
        public string ParameterCode { get; set; }

        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Display On")]
        public string DisplayOn { get; set; }

        [Display(Name = "Is Column Cell Bold")]
        public bool IsColumnCellBold { get; set; }
        public string TestName { get; set; }
        public string CentreCode { get; set; }
    }
}
