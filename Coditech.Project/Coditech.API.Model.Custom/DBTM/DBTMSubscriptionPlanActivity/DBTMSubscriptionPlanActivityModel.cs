using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class DBTMSubscriptionPlanActivityModel : BaseModel
    {
        public int DBTMSubscriptionPlanActivityId { get; set; }         
        public int DBTMSubscriptionPlanId { get; set; }
        public int DBTMTestMasterId { get; set; }
        public bool IsAssociated { get; set; }
        public string PlanName { get; set; }
        public string TestName { get; set; }
        public string TestCode { get; set; }
    }
}
