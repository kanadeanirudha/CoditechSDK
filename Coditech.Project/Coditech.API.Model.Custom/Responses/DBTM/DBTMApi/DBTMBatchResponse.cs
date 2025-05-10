namespace Coditech.Common.API.Model.Responses
{
    public class DBTMBatchResponse : BaseResponse
    {
        public List<DBTMBatchModel> DBTMBatchModel { get; set; }
        public DBTMBatchModel BatchModel { get; set; }
    }
}

