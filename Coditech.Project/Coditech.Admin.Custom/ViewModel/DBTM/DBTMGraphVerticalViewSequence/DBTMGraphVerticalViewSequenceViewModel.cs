using Coditech.Common.API.Model;
using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMGraphVerticalViewSequenceViewModel : BaseViewModel
    {
        public int DBTMGraphVerticalViewSequenceId { get; set; }
        public int DBTMGraphMasterId { get; set; }

        [Display(Name = "Parameter Code")]
        public string ParameterCode { get; set; }

        [Display(Name = "Calculated Parameter")]
        public bool IsCalculatedParameter { get; set; }

        [Required(ErrorMessage = "Recursion is required.")]
        [Range(1, 999, ErrorMessage = "Recursion must be between 1 and 999.")]
        public short Recursion { get; set; }

        [Display(Name = "Sequence Number")]
        public short SequenceNumber { get; set; }

        [Display(Name = "Consecutive Parameter Code")]
        public string ConsecutiveParameterCode { get; set; }

        [Display(Name = "Calculated Consecutive Parameter Code")]
        public bool? IsCalculatedConsecutiveParameterCode { get; set; }

        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }
        [Display(Name = "Column Display Name")]
        public string ColumnDisplayName { get; set; }

        [Display(Name = "Help Text")]
        public string HelpText { get; set; }
        public string DBTMSequenceData { get; set; }
        public List<DBTMGraphVerticalViewSequenceModel> DBTMGraphVerticalViewSequenceList { get; set; }

        [Display(Name = "Display On")]
        public string DisplayOn { get; set; }

        [Display(Name = "Column Cell Color")]
        public string ColumnCellColor { get; set; }

        [Display(Name = "Column Cell Bold")]
        public bool IsColumnCellBold { get; set; }
        public string GraphName { get; set; }
        [Display(Name = "Static Value")]
        public string StaticValue { get; set; }
        [Display(Name = "Common Column")]
        public bool IsCommonColumn { get; set; }
    }
}
