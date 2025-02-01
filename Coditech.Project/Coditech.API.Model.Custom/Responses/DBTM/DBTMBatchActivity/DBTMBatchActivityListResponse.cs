namespace Coditech.Common.API.Model.Response
{
    public class DBTMBatchActivityListResponse : BaseListResponse
    {
        public List<DBTMBatchActivityModel> DBTMBatchActivityList { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public string BatchName { get; set; }
    }
}
