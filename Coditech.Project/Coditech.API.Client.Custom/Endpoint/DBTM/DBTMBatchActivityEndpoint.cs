using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMBatchActivityEndpoint : BaseEndpoint
    {
        public string DBTMBatchActivityListAsync(int generalBatchMasterId, bool isAssociated, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMBatchActivity/GetDBTMBatchActivityList?generalBatchMasterId={generalBatchMasterId}&isAssociated={isAssociated}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }

        public string CreateDBTMBatchActivityAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMBatchActivity/CreateDBTMBatchActivity";

        public string DeleteDBTMBatchActivityAsync() =>
                   $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMBatchActivity/DeleteDBTMBatchActivity";
    }
}
