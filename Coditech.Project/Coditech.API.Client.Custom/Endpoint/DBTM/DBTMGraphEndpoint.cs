using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMGraphEndpoint : BaseEndpoint
    {
        public string ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/GetDBTMGraphList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMGraphAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/CreateDBTMGraph";

        public string GetDBTMGraphAsync(string graphCode) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/GetDBTMGraph?graphCode={graphCode}";
       
        public string UpdateDBTMGraphAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/UpdateDBTMGraph";

        public string DeleteDBTMGraphAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/DeleteDBTMGraph";
        public string GetDBTMGraphTestCodeAsync() =>
          $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/GetDBTMGraphTestCode";
    }
}
