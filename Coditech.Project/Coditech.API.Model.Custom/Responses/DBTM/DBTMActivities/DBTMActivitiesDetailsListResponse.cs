namespace Coditech.Common.API.Model.Response
{
    public class DBTMActivitiesDetailsListResponse : BaseListResponse
    {
        public List<DBTMActivitiesDetailsModel> ActivitiesDetailsList { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonCode { get; set; }
        public string TestName { get; set; }
        public List<string> TestColumns { get; set; }
        public List<string> CalculationColumns { get; set; }
    }
}
