using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMReportsListModel : BaseListModel
    {
        public DataTable DataTable { get; set; }
        public List<KeyValuePair<string, DataTable>> DataTableList { get; set; }
        public DBTMReportsListModel()
        {
            DataTable = new DataTable();
        }
        public string FilePath { get; set; }   
        public string FileName { get; set; }
        public DateTime? TestPerformedTime { get; set; }
        public DBTMReportsListModel DBTMReportsModel { get; set; }
        public string TypeOfRecord { get; set; }
    }
}