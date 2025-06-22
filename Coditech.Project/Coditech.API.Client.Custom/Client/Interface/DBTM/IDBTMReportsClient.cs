using Coditech.Common.API.Model.Response;

namespace Coditech.API.Client
{
    public interface IDBTMReportsClient : IBaseClient
    {
        DBTMBatchWiseReportsListResponse BatchWiseReports(int generalBatchMasterId, DateTime FromDate, DateTime ToDate);
        DBTMTestWiseReportsListResponse TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate,long entityId);
    }
}
