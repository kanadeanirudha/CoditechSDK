using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMSubscriptionPlanActivityViewModel : BaseViewModel
    {
        public int DBTMSubscriptionPlanActivityId { get; set; }
        public int DBTMSubscriptionPlanId { get; set; }
       
        [Display(Name = "Activity")]
        public int DBTMTestMasterId { get; set; }
        public string TestName { get; set; }
        public string TestCode { get; set; }
        [Display(Name = "Is Associated")]
        public bool IsAssociated { get; set; }
        public string PlanName { get; set; }

    }
}
