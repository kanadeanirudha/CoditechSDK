using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMReportsService
    {
        DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
    }
}
