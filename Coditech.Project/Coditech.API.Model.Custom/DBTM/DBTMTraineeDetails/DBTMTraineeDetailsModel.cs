namespace Coditech.Common.API.Model
{
    public class DBTMTraineeDetailsModel : BaseModel
    {
        public long DBTMTraineeDetailId { get; set; }
        public long PersonId { get; set; }
        public string PersonCode { get; set; }
        public string CentreCode { get; set; }
        public string UserType { get; set; }
        public string PastInjuries { get; set; }
        public string MedicalHistory { get; set; }
        public string OtherInformation { get; set; }
        public int? GroupEnumId { get; set; }
        public int? SourceEnumId { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string ImagePath { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public int NumberOfActivityPerformed { get; set; }
    }
}
