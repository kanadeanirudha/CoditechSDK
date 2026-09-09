using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMPrintQREndpoint : BaseEndpoint
    {
        public string DownloadPrintQRAsync(string personIds, int generalBatchMasterId, string templateCode) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrintQR/DownloadPrintQR?personIds={personIds}&generalBatchMasterId={generalBatchMasterId}&templateCode={templateCode}";

        public string GetDBTMPrintQRTraineeListAsync(int generalBatchMasterId, string userType, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrintQR/GetDBTMPrintQRTraineeList?generalBatchMasterId={generalBatchMasterId}&userType={userType}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
    }
}
