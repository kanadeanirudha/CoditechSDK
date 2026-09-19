namespace Coditech.Common.API.Model
{
    public class DBTMReportTraineeProfileListModel : BaseListModel
    {
        public List<DBTMReportTraineeProfileModel> DBTMTraineeProfileList { get; set; }
        public DBTMReportTraineeProfileListModel()
        {
            DBTMTraineeProfileList = new List<DBTMReportTraineeProfileModel>();
        }
        public string OrderBy { get; set; }
    }
}
