using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMCentrewiseTestParameterListView
    {
        [Key]
        public long DBTMCentrewiseTestParameterListViewId { get; set; }
        public int DBTMTestParameterListViewSequenceId { get; set; }
        public string CentreCode { get; set; }
        public string? ColumnName { get; set; }
        public string DisplayOn { get; set; }
        public bool? IsColumnCellBold { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

