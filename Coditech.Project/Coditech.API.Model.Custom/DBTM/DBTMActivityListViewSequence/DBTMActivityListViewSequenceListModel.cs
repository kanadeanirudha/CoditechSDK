namespace Coditech.Common.API.Model
{
    public class DBTMActivityListViewSequenceListModel : BaseListModel
    {
        public List<DBTMActivityListViewSequenceModel> DBTMActivityListViewSequenceList { get; set; }
        public DBTMActivityListViewSequenceListModel()
        {
            DBTMActivityListViewSequenceList = new List<DBTMActivityListViewSequenceModel>();
        }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public string CentreCode { get; set; }
    }
}
