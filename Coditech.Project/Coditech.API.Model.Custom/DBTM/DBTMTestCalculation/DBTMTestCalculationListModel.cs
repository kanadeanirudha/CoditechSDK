namespace Coditech.Common.API.Model
{
    public class DBTMTestCalculationListModel : BaseListModel
    {
        public List<DBTMTestCalculationModel> DBTMTestCalculationList { get; set; }
        public DBTMTestCalculationListModel()
        {
            DBTMTestCalculationList = new List<DBTMTestCalculationModel>();
        }
    }
}
