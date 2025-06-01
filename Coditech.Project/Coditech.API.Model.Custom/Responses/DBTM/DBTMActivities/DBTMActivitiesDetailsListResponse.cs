using System.Data;

namespace Coditech.Common.API.Model.Response
{
    public class DBTMActivitiesDetailsListResponse : BaseListResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonCode { get; set; }
        public string TestName { get; set; }
        public DataTable DataTable { get; set; }
    }
}
