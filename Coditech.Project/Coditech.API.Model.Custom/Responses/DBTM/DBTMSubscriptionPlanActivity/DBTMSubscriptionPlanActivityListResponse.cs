namespace Coditech.Common.API.Model.Response
{
    public class DBTMSubscriptionPlanActivityListResponse : BaseListResponse
    {
        public List<DBTMSubscriptionPlanActivityModel> DBTMSubscriptionPlanActivityList { get; set; }
        public string PlanName { get; set; }
    }
    
}

