using System.Data;

namespace Coditech.Common.API.Model
{
    public class LiveTestResultDashboardModel : BaseModel
    {
        public List<DataTable> DataTableList { get; set; }
        public LiveTestResultDashboardModel()
        {
            DataTableList = new List<DataTable>();
        }
    }
}
