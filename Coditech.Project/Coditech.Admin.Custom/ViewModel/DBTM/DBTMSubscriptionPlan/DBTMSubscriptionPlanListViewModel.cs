using Coditech.Common.Helper;

namespace Coditech.Admin.ViewModel
{
    public class DBTMSubscriptionPlanListViewModel : BaseViewModel
    {
        public List<DBTMSubscriptionPlanViewModel> DBTMSubscriptionPlanList { get; set; }
        public DBTMSubscriptionPlanListViewModel()
        {
            DBTMSubscriptionPlanList = new List<DBTMSubscriptionPlanViewModel>();
        }       
    }
}
