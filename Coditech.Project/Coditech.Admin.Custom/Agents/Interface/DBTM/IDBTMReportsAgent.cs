using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

namespace Coditech.Admin.Agents
{
    public interface IDBTMReportsAgent
    {
        GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate);
    }
}
