using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMReportsListModel : BaseListModel
    {
        public DataTable DataTable { get; set; }
        public DBTMReportsListModel()
        {
            DataTable = new DataTable();
        }
    }
}
