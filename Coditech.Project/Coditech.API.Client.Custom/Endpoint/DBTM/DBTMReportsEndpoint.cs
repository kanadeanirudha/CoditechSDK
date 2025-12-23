using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMReportsEndpoint : BaseEndpoint
    {
        public string BatchWiseMultipleReportsAsync(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/BatchWiseMultipleReports?dBTMTestMasterIds={dBTMTestMasterIds}&generalBatchMasterId={generalBatchMasterId}&fromDate={fromDate}&toDate={toDate}";
            return endpoint;
        }

        public string TestWiseMultipleReportsAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseMultipleReports?dBTMTestMasterIds={dBTMTestMasterIds}&dBTMTraineeDetailId={dBTMTraineeDetailId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }

        public string TestWiseGraphReportsAsync(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseGraphReports?dBTMTestMasterId={dBTMTestMasterId}&dBTMTraineeDetailId={dBTMTraineeDetailId}&dBTMGraphMasterId={dBTMGraphMasterId}&graphMode={graphMode}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string NameWiseReportsAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/NameWiseReports?dBTMTestMasterIds={dBTMTestMasterIds}&dBTMTraineeDetailId={dBTMTraineeDetailId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string TestWiseMultipleReportsFileAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseMultipleReportsFile?dBTMTestMasterIds={dBTMTestMasterIds}&dBTMTraineeDetailId={dBTMTraineeDetailId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}&reportType={reportType}";
            return endpoint;
        }
        public string BatchWiseMultipleReportsFileAsync(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/BatchWiseMultipleReportsFile?dBTMTestMasterIds={dBTMTestMasterIds}&generalBatchMasterId={generalBatchMasterId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}&reportType={reportType}";
            return endpoint;
        }
        public string DeleteReportsFileAsync() =>
                 $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/DeleteReportsFile";
        public string TestWiseGraphReportsV2Async(int dBTMTestMasterId, long dBTMTraineeDetailId, string dBTMGraphMasterIds, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseGraphReportsV2?dBTMTestMasterId={dBTMTestMasterId}&dBTMTraineeDetailId={dBTMTraineeDetailId}&dBTMGraphMasterIds={dBTMGraphMasterIds}&graphMode={graphMode}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string GetActivityPerformedDatesAsync(int dBTMTestMasterId, long dBTMTraineeDetailId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/GetActivityPerformedDates?dBTMTestMasterId={dBTMTestMasterId}&dBTMTraineeDetailId={dBTMTraineeDetailId}";
            return endpoint;
        }
    }
}
