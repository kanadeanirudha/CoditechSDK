namespace Coditech.Common.API.Model
{
    public class DBTMCentreWiseTestListModel : BaseListModel
    {
        public List<DBTMCentreWiseTestModel> DBTMCentreWiseTestList { get; set; }
        public DBTMCentreWiseTestListModel()
        {
            DBTMCentreWiseTestList = new List<DBTMCentreWiseTestModel>();
        }
    }
}
