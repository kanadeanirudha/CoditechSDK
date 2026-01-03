namespace Coditech.Common.API.Model
{
    public class DBTMCentrewiseTestParameterListViewModel : BaseModel
    {
        public long DBTMCentrewiseTestParameterListViewId { get; set; }
        public int DBTMTestParameterListViewSequenceId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string ParameterCode { get; set; }
        public short SequenceNumber { get; set; }
        public string ColumnName { get; set; }
        public bool IsActive { get; set; }
        public List<DBTMCentrewiseTestParameterListViewModel> DBTMCentrewiseTestParameterList { get; set; }
        public string DisplayOn { get; set; }
        public bool IsColumnCellBold { get; set; }
        public string TestName { get; set; }
        public string CentreCode { get; set; }
    }
}
