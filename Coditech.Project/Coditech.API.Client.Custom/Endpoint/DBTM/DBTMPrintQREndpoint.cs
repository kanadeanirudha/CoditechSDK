using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMPrintQREndpoint : BaseEndpoint
    {
        public string DBTMPrintQRAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrintQR/DBTMPrintQR";

        public string GetDBTMPrintQRAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrintQR/GetDBTMPrintQR";

        public string GetDBTMPrintQRTraineeListAsync(int generalBatchMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrintQR/GetDBTMPrintQRTraineeList?generalBatchMasterId={generalBatchMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
    }
}
