namespace Coditech.Common.API.Model.Response
{
    public class DBTMActivitiesDetailsListResponse : BaseListResponse
    {
        public List<DBTMActivitiesDetailsModel> ActivitiesDetailsList { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Columns { get; set; }
    }
}
