namespace Coditech.Common.API.Model
{
    public class DBTMUserModel : BaseModel
    {
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public long EntityId { get; set; }
        public string PersonTitle { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmergencyContact { get; set; }
        public string BloodGroup { get; set; }
        public string MaritalStatus { get; set; }
        public string BirthMark { get; set; }
        public short GeneralOccupationMasterId { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public bool IsPasswordChange { get; set; }
        public string PastInjuries { get; set; }
        public string MedicalHistory { get; set; }
        public string OtherInformation { get; set; }
        public string PhotoMediaPath { get; set; }
        public bool IsAcceptedTermsAndConditions { get; set; }
    }
}
