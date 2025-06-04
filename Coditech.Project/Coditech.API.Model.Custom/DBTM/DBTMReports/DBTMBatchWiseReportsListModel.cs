using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMBatchWiseReportsListModel : BaseListModel
    {
        public DataTable DataTable { get; set; }
        public DBTMBatchWiseReportsListModel()
        {
            DataTable = new DataTable();
        }
    }
}
