using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model;

namespace Coditech.Admin.Agents
{
    public interface IDBTMReportsAgent
    {
        GraphModel TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime FromDate, DateTime ToDate);
        List<GraphModel> TestWiseGraphReportsV2(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, string reportType);
        DBTMReportsListViewModel BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime FromDate, DateTime ToDate, string reportType);
        bool DeleteReportsFile(string fileName);
        List<DateTime> GetActivityPerformedDates(string dBTMTestMasterIds, long dBTMTraineeDetailId);
        List<DateTime> GetBatchActivityPerformedDates(string dBTMTestMasterIds, int generalBatchMasterId);
        DBTMReportVerticalDataViewModel GetActivityVerticalDetails(long dBTMDeviceDataId);
        DBTMTraineeProfileListViewModel GetBatchWiseTraineeProfileDetailsList(long generalBatchMasterId, string dbtmTraineeDetailIds, string orderBy);
        DBTMReportsListViewModel CampWiseMultipleReports(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime FromDate, DateTime ToDate);
        DBTMReportsListViewModel CampWiseMultipleReportsFile(string dBTMTestMasterIds, int dBTMCampMasterId, DateTime FromDate, DateTime ToDate, string ReportType);
        List<DateTime> GetCampActivityPerformedDates(string dBTMTestMasterIds, int dBTMCampMasterId);
        List<DateTime> GetTraineeListActivityDates(string dbtmTraineeDetailIds, int generalBatchMasterId);
    }
}
