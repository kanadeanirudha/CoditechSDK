using System.Data;
using Microsoft.AspNetCore.Http;
namespace Coditech.Common.API.Model
{
    public class DBTMTraineeUploadModel : BaseModel
    {
        public int TotalRecords { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public DataTable DataTable { get; set; }
        public List<Dictionary<string, object>> FailedRows { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
    }
}