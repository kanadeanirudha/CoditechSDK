using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMReportsAgent
    {
        DBTMBatchWiseReportsListViewModel BatchWiseReports(int generalBatchMasterId);
        DBTMTestWiseReportsListViewModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
    }
}
