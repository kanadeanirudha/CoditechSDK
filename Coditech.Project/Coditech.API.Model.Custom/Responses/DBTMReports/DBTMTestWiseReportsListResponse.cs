using System.Data;

namespace Coditech.Common.API.Model.Response
{
    public class DBTMTestWiseReportsListResponse : BaseListResponse
    {
        public DataTable DataTable { get; set; }
        public List<KeyValuePair<string, DataTable>> DataTableList { get; set; }
    }
}
