namespace Coditech.Common.API.Model.Response
{
    public class DBTMCampUserListResponse : BaseListResponse
    {
        public List<DBTMCampUserModel> DBTMCampUserList { get; set; }
        public long DBTMCampMasterId { get; set; }
        public string CampName { get; set; }
    }
}
