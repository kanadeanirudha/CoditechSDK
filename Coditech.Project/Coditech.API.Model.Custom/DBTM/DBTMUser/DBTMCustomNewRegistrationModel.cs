namespace Coditech.Common.API.Model
{
    public class DBTMCustomNewRegistrationModel : BaseModel
    {
        public decimal height { get; set; }
        public decimal weight { get; set; }
        public int? SpecializationEnumId { get; set; }
        public List<string> GeneralTraineeAssociatedToTrainerIds { get; set; }
        public string JoiningCode { get; set; }
        public int GeneralBatchMasterId { get; set; }
        public int DBTMCampMasterId { get; set; }
        public string SchoolName { get; set; }
        public string AgeGroup { get; set; }
    }
}
