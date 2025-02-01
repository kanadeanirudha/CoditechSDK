using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMMySubscriptionPlanListViewModel : BaseViewModel
    {
        public List<DBTMSubscriptionPlanViewModel> DBTMMySubscriptionPlanList { get; set; }
        public DBTMMySubscriptionPlanListViewModel()
        {
            DBTMMySubscriptionPlanList = new List<DBTMSubscriptionPlanViewModel>();
        }
    }
}
