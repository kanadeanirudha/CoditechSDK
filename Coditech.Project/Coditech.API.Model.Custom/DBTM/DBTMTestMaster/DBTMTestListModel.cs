namespace Coditech.Common.API.Model
{
    public class DBTMTestListModel : BaseListModel
    {
        public List<DBTMTestModel> DBTMTestList { get; set; }
        public DBTMTestListModel()
        {
            DBTMTestList = new List<DBTMTestModel>();
        }
    }
}
