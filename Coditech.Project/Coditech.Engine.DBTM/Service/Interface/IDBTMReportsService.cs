using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMReportsService
    {
        DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, long entityId);
    }
}
