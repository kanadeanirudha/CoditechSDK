namespace Coditech.Common.API.Model
{
    public class DBTMSubscriptionPlanActivityListModel : BaseListModel
    {
        public List<DBTMSubscriptionPlanActivityModel> DBTMSubscriptionPlanActivityList { get; set; }
        public DBTMSubscriptionPlanActivityListModel()
        {
            DBTMSubscriptionPlanActivityList = new List<DBTMSubscriptionPlanActivityModel>();
        }
        public string PlanName { get; set; }
        public string TestName { get; set; }
        public int DBTMSubscriptionPlanId { get; set; }
    }
}

