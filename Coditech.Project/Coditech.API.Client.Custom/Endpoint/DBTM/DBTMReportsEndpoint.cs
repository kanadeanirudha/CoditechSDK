using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMReportsEndpoint : BaseEndpoint
    {
        public string BatchWiseReportsAsync(int generalBatchMasterId, int dBTMTestMasterId, DateTime FromDate,DateTime ToDate)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/BatchWiseReports?generalBatchMasterId={generalBatchMasterId}&dBTMTestMasterId={dBTMTestMasterId}&FromDate={FromDate}&ToDate={ToDate}";
            return endpoint;
        }

        public string TestWiseReportsAsync(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, long UserMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/TestWiseReports?dBTMTestMasterId={dBTMTestMasterId}&dBTMTraineeDetailId={dBTMTraineeDetailId}&FromDate={FromDate}&ToDate={ToDate}&UserMasterId={UserMasterId}";
            return endpoint;
        }
    }
}
