using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMBatchEndpoint : BaseEndpoint
    {
        public string DBTMBatchAsync(long entityId, string userType)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApi/GetBatchList?entityId={entityId}&userType={userType}";
            return endpoint;
        }
    }
}
