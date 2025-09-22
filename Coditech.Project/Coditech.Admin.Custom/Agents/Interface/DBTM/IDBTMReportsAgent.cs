using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

namespace Coditech.Admin.Agents
{
    public interface IDBTMReportsAgent
    {
        DBTMReportsListViewModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
        GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
    }
}
