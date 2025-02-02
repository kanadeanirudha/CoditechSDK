using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMSubscriptionPlanActivityListViewModel : BaseViewModel
    {
        public List<DBTMSubscriptionPlanActivityViewModel> DBTMSubscriptionPlanActivityList { get; set; }

        public DBTMSubscriptionPlanActivityListViewModel()
        {
            DBTMSubscriptionPlanActivityList = new List<DBTMSubscriptionPlanActivityViewModel>();
        }
        public int DBTMSubscriptionPlanId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public string PlanName { get; set; }
        public string SelectedParameter1 { get; set; }
    
    }
}
