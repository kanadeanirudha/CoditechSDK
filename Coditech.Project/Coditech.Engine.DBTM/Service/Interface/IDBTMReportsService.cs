using Coditech.Common.API.Model;
using System.Data;

namespace Coditech.API.Service
{
    public interface IDBTMReportsService
    {
        DBTMReportsListModel BatchWiseReports(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate, bool isMobileRequest, bool isDownloadReport);
        DBTMReportsListModel TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string typeOfRecord);
        List<GraphModel> TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string typeOfRecord);
        DBTMReportsListModel NameWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest);
        DBTMReportsListModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, bool isDownloadReport);
        DBTMReportsListModel BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, bool isMobileRequest);
        DBTMReportsListModel TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType);
        DBTMReportsListModel BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType);
        bool DeleteReportsFile(string fileName);
        List<DateTime> GetActivityPerformedDates(string dBTMTestMasterIds, long dBTMTraineeDetailId, string centreCode);
        DBTMReportVerticalDataModel GetActivityVerticalDetails(long dBTMDeviceDataId, string typeOfRecord);
        GeneralBatchUserListModel GetBatchWiseUser(long generalBatchMasterId);
        List<DateTime> GetBatchActivityPerformedDates(string dBTMTestMasterIds, int generalBatchMasterId);
        DBTMReportsListModel CampWiseMultipleReports(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime fromDate, DateTime toDate, bool isMobileRequest);
        DBTMReportsListModel CampWiseMultipleReportsFile(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, bool isMobileRequest, string reportType);
        List<DateTime> GetCampActivityPerformedDates(string dBTMTestMasterIds, int dBTMCampMasterId);
        DataTable GetLiveResultDataTable(int dBTMTestMasterId, string centreCode, List<DBTMReportsModel> dBTMReportsList, DateTime fromDate, DateTime toDate);
        DBTMReportsListModel AssignmentWiseMultipleReports(string dBTMTestMasterIds, long generalTrainerMasterId, DateTime fromDate, DateTime toDate, bool isMobileRequest);
        DBTMReportsListModel AssignmentWiseMultipleReportsV2(string dBTMTestMasterIds, string dBTMTraineeDetailIds, long generalTrainerMasterId, DateTime fromDate, DateTime toDate, bool isMobileRequest);
        DBTMReportTraineeProfileListModel GetProfileDetailsList(long generalBatchMasterId, string dBTMTraineeDetailIds, string orderBy, DateTime FromDate, DateTime ToDate);

    }
}
