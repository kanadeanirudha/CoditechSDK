using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMSubscriptionPlan
    {
        [Key]
        public int DBTMSubscriptionPlanId { get; set; }
        public string PlanName { get; set; }
        public short DurationInDays { get; set; }
        public decimal PlanCost { get; set; }
        public decimal PlanDiscount { get; set; }
        public int SubscriptionPlanTypeEnumId { get; set; }
        public bool IsActive { get; set; }
        public bool IsTaxExclusive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

