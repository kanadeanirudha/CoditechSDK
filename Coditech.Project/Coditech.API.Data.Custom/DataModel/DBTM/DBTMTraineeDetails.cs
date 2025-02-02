using System.ComponentModel.DataAnnotations;

namespace Coditech.API.Data
{
    public partial class DBTMTraineeDetails
    {
        [Key]
        public long DBTMTraineeDetailId { get; set; }
        public string CentreCode { get; set; }
        public long PersonId { get; set; }
        public string PersonCode { get; set; }
        public string UserType { get; set; }
        public string PastInjuries { get; set; }
        public string MedicalHistory { get; set; }
        public string OtherInformation { get; set; }
        public int? GroupEnumId { get; set; }
        public int? SourceEnumId { get; set; }
        public bool IsActive { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}

