namespace Coditech.Common.API.Model
{
    public class DBTMMySubscriptionPlanListModel : BaseListModel
    {
        public List<DBTMSubscriptionPlanModel> DBTMMySubscriptionPlanList { get; set; }
        public DBTMMySubscriptionPlanListModel()
        {
            DBTMMySubscriptionPlanList = new List<DBTMSubscriptionPlanModel>();
        }
    }
}
