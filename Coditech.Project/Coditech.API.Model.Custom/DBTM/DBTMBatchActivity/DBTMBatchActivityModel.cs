namespace Coditech.Common.API.Model
{
    public class DBTMBatchActivityModel : BaseModel
    {
        public long DBTMBatchActivityId { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public bool IsAssociated { get; set; }
        public string BatchName { get; set; }
    }
}
