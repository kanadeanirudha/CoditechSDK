using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMReportsEndpoint : BaseEndpoint
    {
        public string BatchWiseReportsAsync(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate, DateTime ToDate)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/BatchWiseReports?generalBatchMasterId={generalBatchMasterId}&dBTMTestMasterId={dBTMTestMasterId}&FromDate={FromDate}&ToDate={ToDate}";
            return endpoint;
        }

        public string TestWiseReportsAsync(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseReports?dBTMTestMasterId={dBTMTestMasterId}&dBTMTraineeDetailId={dBTMTraineeDetailId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string TestWiseGraphReportsAsync(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseGraphReports?dBTMTestMasterId={dBTMTestMasterId}&dBTMTraineeDetailId={dBTMTraineeDetailId}&dBTMGraphMasterId={dBTMGraphMasterId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string NameWiseReportsAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/NameWiseReports?dBTMTestMasterIds={dBTMTestMasterIds}&dBTMTraineeDetailId={dBTMTraineeDetailId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string TestWiseMultipleReportsAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseMultipleReports?dBTMTestMasterIds={dBTMTestMasterIds}&dBTMTraineeDetailId={dBTMTraineeDetailId}&fromDate={fromDate}&toDate={toDate}&entityId={entityId}&userType={userType}&centreCode={centreCode}";
            return endpoint;
        }
        public string BatchWiseMultipleReportsAsync(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/BatchWiseMultipleReports?dBTMTestMasterIds={dBTMTestMasterIds}&generalBatchMasterId={generalBatchMasterId}&fromDate={fromDate}&toDate={toDate}";
            return endpoint;
        }
    }
}
