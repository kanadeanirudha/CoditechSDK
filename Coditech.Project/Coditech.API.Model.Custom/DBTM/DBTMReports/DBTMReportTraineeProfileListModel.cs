namespace Coditech.Common.API.Model
{
    public class DBTMReportTraineeProfileListModel : BaseListModel
    {
        public List<DBTMReportTraineeProfileModel> DBTMTraineeProfileList { get; set; }
        public List<DBTMReportPerformanceStandardModel> DBTMReportPerformanceStandardList { get; set; }
        public DBTMReportTraineeProfileListModel()
        {
            DBTMTraineeProfileList = new List<DBTMReportTraineeProfileModel>();
            DBTMReportPerformanceStandardList = new List<DBTMReportPerformanceStandardModel>();
        }
        public string OrderBy { get; set; }
    }
}
