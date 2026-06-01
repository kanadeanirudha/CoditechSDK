namespace Coditech.Common.API.Model.Responses
{
    public class DBTMOrganisationCentrewiseJoiningCodeResponse : BaseResponse
    {
        public DBTMOrganisationCentrewiseJoiningCodeModel DBTMOrganisationCentrewiseJoiningCodeModel { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string JoiningCode { get; set; }
        public bool IsInQueue { get; set; }
        public DateTime? QueueValidTill { get; set; }
    }
}

