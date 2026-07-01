using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;

namespace Coditech.API.Client
{
    public interface IDBTMReportsClient : IBaseClient
    {
        GraphResponse TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
        GraphListResponse TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
        DBTMTestWiseReportsListResponse NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
        DBTMTestWiseReportsListResponse TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
        DBTMTestWiseReportsListResponse BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate);
        DBTMTestWiseReportsListResponse TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType);
        DBTMTestWiseReportsListResponse BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType);
        DBTMTraineeProfileListResponse GetBatchWiseTraineeProfileDetailsList(long generalBatchMasterId, string dbtmTraineeDetailIds, string orderBy, DateTime FromDate, DateTime ToDate);
        TrueFalseResponse DeleteReportsFile(ParameterModel body);
        List<string> GetActivityPerformedDates(string dBTMTestMasterIds, long dBTMTraineeDetailId, string centreCode);
        List<string> GetBatchActivityPerformedDates(string dBTMTestMasterIds, int generalBatchMasterId);
        DBTMReportVerticalDataResponse GetActivityVerticalDetails(long dBTMDeviceDataId, string typeOfRecord);
        DBTMTestWiseReportsListResponse CampWiseMultipleReports(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode);
        DBTMTestWiseReportsListResponse CampWiseMultipleReportsFile(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime fromDate, DateTime toDate, long entityId,  string userType, string centreCode, string reportType);
        List<string> GetCampActivityPerformedDates(string dBTMTestMasterIds, int dBTMCampMasterId);
        List<string> GetTraineeListActivityDates(string dBTMTraineeDetailIds, int generalBatchMasterId);
    }
}
