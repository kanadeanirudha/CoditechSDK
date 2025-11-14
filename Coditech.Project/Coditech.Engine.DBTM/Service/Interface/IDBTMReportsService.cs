using Coditech.Common.API.Model;

namespace Coditech.API.Service
{
    public interface IDBTMReportsService
    {
        DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate, bool isMobileRequest);
        DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        List<GraphModel> TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        DBTMReportsListModel NameWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        DBTMReportsListModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        DBTMReportsListModel BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, bool isMobileRequest);
        DBTMReportsListModel TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType);
        DBTMReportsListModel BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType);
        bool DeleteReportsFile(string fileName);
    }
}
