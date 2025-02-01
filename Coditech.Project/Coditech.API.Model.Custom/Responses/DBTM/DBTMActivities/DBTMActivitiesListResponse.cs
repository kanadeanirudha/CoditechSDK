namespace Coditech.Common.API.Model.Response
{
    public class DBTMActivitiesListResponse : BaseListResponse
    {
        public List<DBTMActivitiesModel> ActivitiesList { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
