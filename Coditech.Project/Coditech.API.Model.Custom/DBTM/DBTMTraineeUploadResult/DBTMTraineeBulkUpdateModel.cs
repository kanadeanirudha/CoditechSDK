namespace Coditech.Common.API.Model
{
    public class DBTMTraineeBulkUpdateModel : BaseModel
    {
        public string PersonCode { get; set; }
        public string TraineeTitle { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal? HeightCm { get; set; }
        public decimal? WeightKg { get; set; }
        public string Specialization { get; set; }
        public string SchoolOrCollegeOrClub { get; set; }
        public string AgeGroup { get; set; }
    }
}
