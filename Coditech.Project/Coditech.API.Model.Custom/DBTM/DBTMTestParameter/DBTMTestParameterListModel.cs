namespace Coditech.Common.API.Model
{
    public class DBTMTestParameterListModel : BaseListModel
    {
        public List<DBTMTestParameterModel> DBTMTestParameterList { get; set; }
        public DBTMTestParameterListModel()
        {
            DBTMTestParameterList = new List<DBTMTestParameterModel>();
        }

    }
}
