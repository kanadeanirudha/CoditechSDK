using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivityListViewSequenceViewModel : BaseViewModel
    {
        public int DBTMTestParameterListViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Parameter Code")]
        public string ParameterCode { get; set; }

        [Display(Name = "Is Calculated Parameter")]
        public bool IsCalculatedParameter { get; set; }


        [Display(Name = "Recursion")]
        public short Recursion { get; set; }

        [Display(Name = "Sequence Number")]
        public short SequenceNumber { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Consecutive Parameter Code")]
        public string ConsecutiveParameterCode { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Is Calculated Consecutive Parameter Code")]
        public bool IsCalculatedConsecutiveParameterCode { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Column Name")]
        public string ColumnName { get; set; }
    }
}
