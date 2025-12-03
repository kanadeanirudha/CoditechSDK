namespace Coditech.Common.API.Model.Response
{
    public class DBTMNewRegistrationListResponse : BaseListResponse
    {
        public List<DBTMNewRegistrationModel> DBTMNewRegistrationList { get; set; }
        public string JoiningCode { get; set; }
    }
}
