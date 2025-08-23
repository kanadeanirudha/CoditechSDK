namespace Coditech.Common.API.Model
{
    public class DBTMTrainerDetailsModel : BaseModel
    {
        public DBTMTrainerDetailsModel()
        {
        }
        public long GeneralTrainerMasterId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeDesignation { get; set; }
        public string TrainerSpecialization { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string DurationWithUs { get; set; }
        public int NumberOfTrainees { get; set; }
        public int NumberOfBatchesCreated { get; set; }
        public TimeSpan WeeklyWorkingHours { get; set; }
        public long PhotoMediaId { get; set; }
        public string PhotoMediaPath { get; set; }
        public string PhotoMediaFileName { get; set; }
        public long PersonId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public string CallingCode { get; set; }
        public string MobileNumber { get; set; }
        public int AdminRoleMasterId { get; set; }
        public long UserMasterId { get; set; }
    }
}
