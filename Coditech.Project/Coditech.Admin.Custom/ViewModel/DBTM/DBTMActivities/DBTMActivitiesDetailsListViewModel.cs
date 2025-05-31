using Coditech.Common.Helper;
using System.Data;

namespace Coditech.Admin.ViewModel
{
    public class DBTMActivitiesDetailsListViewModel : BaseViewModel
    {
        public DataTable DataTable { get; set; }
        public DBTMActivitiesDetailsListViewModel()
        {
        }
        public string PersonCode { get; set; }
        public long DBTMDeviceDataId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SelectedParameter1 { get; set; }
        public string TestName { get; set; }
    }
}
