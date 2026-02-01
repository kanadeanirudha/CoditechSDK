namespace Coditech.Common.API.Model
{
    public class DBTMActivityVerticalViewSequenceListModel : BaseListModel
    {
        public List<DBTMActivityVerticalViewSequenceModel> DBTMActivityVerticalViewSequenceList { get; set; }
        public DBTMActivityVerticalViewSequenceListModel()
        {
            DBTMActivityVerticalViewSequenceList = new List<DBTMActivityVerticalViewSequenceModel>();
        }
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public string CentreCode { get; set; }
    }
}
