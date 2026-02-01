using System.Data;

namespace Coditech.Common.API.Model.Responses
{
    public class DBTMTraineeUploadResponse : BaseResponse
    {
        public DBTMTraineeUploadModel DBTMTraineeUploadModel { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}

