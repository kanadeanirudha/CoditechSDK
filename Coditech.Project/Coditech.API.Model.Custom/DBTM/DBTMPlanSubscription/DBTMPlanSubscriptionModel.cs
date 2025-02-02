namespace Coditech.Common.API.Model
{
    public class DBTMSubscriptionPlanModel : BaseModel
    {
        public int DBTMSubscriptionPlanId { get; set; }
        public string PlanName { get; set; }
        public short DurationInDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsTaxExclusive { get; set; }
        public decimal PlanCost { get; set; }       
        public decimal PlanDiscount { get; set; }
        public int SubscriptionPlanTypeEnumId { get; set; }
        public string SubscriptionPlanType { get; set; }
        public long DBTMDeviceMasterId { get; set; }
        public DateTime PlanDurationExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public string DeviceSerialCode { get; set; }

    }
}
