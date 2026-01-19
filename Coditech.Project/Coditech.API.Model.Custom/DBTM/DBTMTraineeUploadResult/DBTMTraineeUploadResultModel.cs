namespace Coditech.Common.API.Model
{
    public class DBTMTraineeUploadResultModel : BaseModel
    {
        public int TotalRecords { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
    }
}
