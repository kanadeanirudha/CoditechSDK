namespace Coditech.Common.API.Model
{
    public class DBTMBatchModel
    {
        public int GeneralBatchMasterId { get; set; }
        public string BatchName { get; set; }
        public TimeSpan BatchTime { get; set; }
        public TimeSpan BatchStartTime { get; set; }
        public List<DBTMTestApiModel> DBTMBatchTestList { get; set; }
        public List<DBTMGeneralBatchUserModel> DBTMGeneralBatchUserModel { get; set; }
    }
}

