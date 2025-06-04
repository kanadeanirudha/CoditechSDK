using System.Data;

namespace Coditech.Common.API.Model.Response
{
    public class DBTMBatchWiseReportsListResponse : BaseListResponse
    {
        public DataTable DataTable { get; set; }
    }
}
