using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMReportsAgent
    {
        DBTMBatchWiseReportsListViewModel BatchWiseReports(int generalBatchMasterId, DateTime FromDate, DateTime ToDate);
        DBTMTestWiseReportsListViewModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
    }
}
