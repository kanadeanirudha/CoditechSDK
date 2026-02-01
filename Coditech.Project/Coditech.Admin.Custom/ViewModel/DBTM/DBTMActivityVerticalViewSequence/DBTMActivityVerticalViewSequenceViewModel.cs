using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivityVerticalViewSequenceViewModel : BaseViewModel
    {
        public int DBTMTestParameterVerticalViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }

        [Display(Name = "Parameter Code")]
        public string ParameterCode { get; set; }

        [Display(Name = "Is Calculated Parameter")]
        public bool IsCalculatedParameter { get; set; }

        [Required(ErrorMessage = "Recursion is required.")]
        [Range(1, 999, ErrorMessage = "Recursion must be between 1 and 999.")]
        public short Recursion { get; set; }

        [Display(Name = "Sequence Number")]
        public short SequenceNumber { get; set; }

        [Display(Name = "Consecutive Parameter Code")]
        public string ConsecutiveParameterCode { get; set; }

        [Display(Name = "Is Calculated Consecutive Parameter Code")]
        public bool? IsCalculatedConsecutiveParameterCode { get; set; }

        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }

        [Display(Name = "Help Text")]
        public string HelpText { get; set; }
        public string DBTMSequenceData { get; set; }
        public List<DBTMActivityVerticalViewSequenceModel> DBTMActivityVerticalViewSequenceList { get; set; }

        [Display(Name = "Display On")]
        public string DisplayOn { get; set; }

        [Display(Name = "Column Cell Color")]
        public string ColumnCellColor { get; set; }

        [Display(Name = "Is Column Cell Bold")]
        public bool IsColumnCellBold { get; set; }
        public string TestName { get; set; }
        [Display(Name = "Static Value")]
        public string StaticValue { get; set; }
    }
}
