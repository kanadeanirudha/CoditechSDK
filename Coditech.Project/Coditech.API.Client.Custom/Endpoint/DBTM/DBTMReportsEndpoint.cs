using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMReportsEndpoint : BaseEndpoint
    {
        public string BatchWiseReportsAsync(int generalBatchMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMReports/BatchWiseReports?generalBatchMasterId={generalBatchMasterId}";
            return endpoint;
        }
    }
}
