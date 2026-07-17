using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMApplicationVersionEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApplicationVersion/GetDBTMApplicationVersionList?{BuildEndpointQueryString(false, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string GetDBTMApplicationVersionAsync(long dBTMApplicationVersionId) =>
          $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApplicationVersion/GetDBTMApplicationVersion?dBTMActivityCategoryId={dBTMApplicationVersionId}";
        public string CreateDBTMApplicationVersionAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApplicationVersion/CreateDBTMApplicationVersion";
        public string UpdateDBTMApplicationVersionAsync() =>
              $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApplicationVersion/UpdateDBTMApplicationVersion";

        public string DeleteDBTMApplicationVersionAsync() =>
                   $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMApplicationVersion/DeleteDBTMApplicationVersion";
    }
}
