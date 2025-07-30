using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMGraphListModel : BaseListModel
    {
        public DataTable DataTable { get; set; }
        public DBTMGraphListModel()
        {
            DataTable = new DataTable();
        }
    }
}
