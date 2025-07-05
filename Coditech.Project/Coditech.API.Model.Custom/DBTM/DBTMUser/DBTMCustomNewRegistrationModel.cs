namespace Coditech.Common.API.Model
{
    public class DBTMCustomNewRegistrationModel : BaseModel
    {
        public decimal height { get; set; }
        public decimal weight { get; set; }
        public List<string> GeneralTraineeAssociatedToTrainerIds { get; set; }
    }
}
