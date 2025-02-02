namespace Coditech.Common.API.Model
{
    public class DBTMActivitiesListModel : BaseListModel
    {
        public List<DBTMActivitiesModel> ActivitiesList { get; set; }
        public DBTMActivitiesListModel()
        {
            ActivitiesList = new List<DBTMActivitiesModel>();
        }
        public string PersonCode { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
