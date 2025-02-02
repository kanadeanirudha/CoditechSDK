using Coditech.Common.Helper;
using System.ComponentModel.DataAnnotations;

namespace Coditech.Admin.ViewModel
{
    public class DBTMSubscriptionPlanViewModel : BaseViewModel
    {
        public int DBTMSubscriptionPlanId { get; set; }
        [Required]
        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }
        [Required]
        [Display(Name = "Duration In Days")]
        public short DurationInDays { get; set; }
        [Required]
        [Display(Name = "Plan Cost")]
        public Nullable<decimal> PlanCost { get; set; }
        [Required]
        [Display(Name = "Plan Discount")]
        public Nullable<decimal> PlanDiscount { get; set; }
        [Required]
        [Display(Name = "Subscription Plan Type")]
        public int SubscriptionPlanTypeEnumId { get; set; }
        [Display(Name = "Subscription Plan Type")]
        public string SubscriptionPlanType { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Is Tax Exclusive")]
        public bool IsTaxExclusive { get; set; }
        public long DBTMDeviceMasterId { get; set; }
        public DateTime PlanDurationExpirationDate { get; set; }
        public bool IsExpired { get; set; }
        public string DeviceSerialCode { get; set; }
    }
}
