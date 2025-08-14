namespace Coditech.Common.API.Model
{
    public class DBTMPerformanceMatrixListModel : BaseListModel
    {
        public List<DBTMPerformanceMatrixModel> DBTMPerformanceMatrixList { get; set; }
        public DBTMPerformanceMatrixListModel()
        {
            DBTMPerformanceMatrixList = new List<DBTMPerformanceMatrixModel>();
        }
    }
}
