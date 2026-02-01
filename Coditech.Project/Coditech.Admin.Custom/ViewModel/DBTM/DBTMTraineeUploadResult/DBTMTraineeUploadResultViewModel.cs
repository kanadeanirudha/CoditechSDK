using System.Data;
using Coditech.Common.API.Model;
using Coditech.Common.Helper;
namespace Coditech.Admin.ViewModel
{
    public class DBTMTraineeUploadResultViewModel : BaseViewModel
    {
        public int TotalRecords { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<Dictionary<string, object>> FailedRows { get; set; }
        public List<Dictionary<string, object>> Data { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}