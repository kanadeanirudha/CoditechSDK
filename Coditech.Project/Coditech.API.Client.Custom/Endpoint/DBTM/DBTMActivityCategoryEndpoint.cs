using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMActivityCategoryEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMActivityCategory/GetDBTMActivityCategoryList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMActivityCategoryAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMActivityCategory/CreateDBTMActivityCategory";

        public string GetDBTMActivityCategoryAsync(short dBTMActivityCategoryId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMActivityCategory/GetDBTMActivityCategory?dBTMActivityCategoryId={dBTMActivityCategoryId}";

        public string UpdateDBTMActivityCategoryAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMActivityCategory/UpdateDBTMActivityCategory";

        public string DeleteDBTMActivityCategoryAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMActivityCategory/DeleteDBTMActivityCategory";
    }
}
