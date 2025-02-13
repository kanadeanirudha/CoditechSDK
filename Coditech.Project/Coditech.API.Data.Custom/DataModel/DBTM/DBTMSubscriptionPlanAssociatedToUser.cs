using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMSubscriptionPlanAssociatedToUser
    {
        [Key]
        public long DBTMSubscriptionPlanAssociatedToUserId { get; set; }
        public int DBTMSubscriptionPlanId { get; set; }
        public string UserType { get; set; }
        public long EntityId { get; set; }
        public long DBTMDeviceMasterId { get; set; }
        public short DurationInDays { get; set; }
        public decimal PlanCost { get; set; }
        public decimal PlanDiscount { get; set; }
        public bool IsExpired { get; set; }
        public DateTime PlanDurationExpirationDate { get; set; }
        public long SalesInvoiceMasterId { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

