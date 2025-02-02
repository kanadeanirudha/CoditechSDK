namespace Coditech.Common.API.Model
{
    public class DBTMSubscriptionPlanListModel : BaseListModel
    {
        public List<DBTMSubscriptionPlanModel> DBTMSubscriptionPlanList { get; set; }
        public DBTMSubscriptionPlanListModel()
        {
            DBTMSubscriptionPlanList = new List<DBTMSubscriptionPlanModel>();
        }

    }
}
