using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMTestEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMTestList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMTestAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/CreateDBTMTest";

        public string GetDBTMTestAsync(int dBTMTestMasterId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMTest?dBTMTestMasterId={dBTMTestMasterId}";

        public string UpdateDBTMTestAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/UpdateDBTMTest";

        public string DeleteDBTMTestAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/DeleteDBTMTest";

        public string GetDBTMTestParameterAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMTestParameter";
    }
}
