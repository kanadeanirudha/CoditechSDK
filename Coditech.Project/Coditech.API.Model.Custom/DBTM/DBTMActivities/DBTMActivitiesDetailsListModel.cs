using System.Data;

namespace Coditech.Common.API.Model
{
    public class DBTMActivitiesDetailsListModel : BaseListModel
    {
        public DataTable DataTable { get; set; }
        public DBTMActivitiesDetailsListModel()
        {
            DataTable = new DataTable();
        }
        public string PersonCode { get; set; }
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TestName { get; set; }
    }
}
