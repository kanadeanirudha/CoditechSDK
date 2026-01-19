namespace Coditech.Common.API.Model
{
    public class DBTMTraineeUploadRowModel
    {
        public string JoiningCode { get; set; }
        public string PersonTitle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CallingCode { get; set; }
        public string MobileNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal HeightCm { get; set; }
        public decimal WeightKg { get; set; }
        public int SpecializationEnumId { get; set; }
    }
}
