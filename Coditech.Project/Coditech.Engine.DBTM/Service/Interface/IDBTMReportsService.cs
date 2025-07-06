using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMReportsService
    {
        DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, long UserMasterId);
    }
}
