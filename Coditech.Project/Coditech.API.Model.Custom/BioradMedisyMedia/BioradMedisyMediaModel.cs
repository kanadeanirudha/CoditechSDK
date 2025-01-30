using System.ComponentModel.DataAnnotations;

namespace Coditech.Common.API.Model
{
    public class BioradMedisyMediaModel : MediaModel
    {
        public List<BioradMedisyFileHistoryModel> FileHistoryList { get; set; }
        public int TaskApprovalStatusEnumId { get; set; }
        [Required]
        [Display(Name = "Suggestion")]
        public string Comments { get; set; }
        public bool IsFinalApproval { get; set; }
        public bool IsEditable { get; set; }
        public byte ApprovalSequenceNumber { get; set; }
        public string TaskApprovalStatusDisplayName { get; set; }
        public string TaskApprovalStatusEnumCode { get; set; }
    }

    public class BioradMedisyFileHistoryModel
    {
        public int TaskApprovalSettingId { get; set; }
        public string UserName { get; set; }
        public int TaskApprovalStatusEnumId { get; set; }
        public string TaskApprovalStatusDisplayName { get; set; }
        public string TaskApprovalStatusEnumCode { get; set; }
        public string Comments { get; set; }
        public bool IsCurrentStatus { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
