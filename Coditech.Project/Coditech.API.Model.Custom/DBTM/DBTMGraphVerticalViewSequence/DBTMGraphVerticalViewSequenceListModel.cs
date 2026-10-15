namespace Coditech.Common.API.Model
{
    public class DBTMGraphVerticalViewSequenceListModel : BaseListModel
    {
        public List<DBTMGraphVerticalViewSequenceModel> DBTMGraphVerticalViewSequenceList { get; set; }
        public DBTMGraphVerticalViewSequenceListModel()
        {
            DBTMGraphVerticalViewSequenceList = new List<DBTMGraphVerticalViewSequenceModel>();
        }
        public int DBTMGraphMasterId { get; set; }
        public string GraphName { get; set; }
        public string GraphCode { get; set; }
        public string CentreCode { get; set; }
    }
}
