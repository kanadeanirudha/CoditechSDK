namespace Coditech.Common.API.Model
{
    public class DBTMOrganisationCentrewiseJoiningCodeModel : BaseModel
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string JoiningCode { get; set; }
        public bool IsInQueue { get; set; }
        public DateTime? QueueValidTill { get; set; }
    }
}
