namespace Coditech.Common.API.Model
{
    public class DBTMActivitiesDetailsListModel : BaseListModel
    {
        public List<DBTMActivitiesDetailsModel> ActivitiesDetailsList { get; set; }
        public List<string> Columns { get; set; }
        public DBTMActivitiesDetailsListModel()
        {
            ActivitiesDetailsList = new List<DBTMActivitiesDetailsModel>();
            Columns = new List<string>();
        }
        public string PersonCode { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TestName { get; set; }
    }
}
