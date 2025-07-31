using Coditech.Common.API.Model.Response;

namespace Coditech.API.Client
{
    public interface IDBTMReportsClient : IBaseClient
    {
        DBTMBatchWiseReportsListResponse BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate);
        DBTMTestWiseReportsListResponse TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
        DBTMTestWiseReportsListResponse TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
    }
}
