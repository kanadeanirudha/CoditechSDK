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
        public string GetGraphVerticalViewSequenceListAsync(int dBTMGraphMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/GetGraphVerticalViewSequenceList?dBTMGraphMasterId={dBTMGraphMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
        }
        public string DeleteGraphVerticalViewSequenceAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/DeleteGraphVerticalViewSequence";
        public string GetGraphVerticalViewSequenceAsync(int dBTMGraphVerticalViewSequenceId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/GetGraphVerticalViewSequence?dBTMGraphVerticalViewSequenceId={dBTMGraphVerticalViewSequenceId}";
        public string UpdateGraphVerticalViewSequenceAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/UpdateGraphVerticalViewSequence";
        public string UpdateGraphVerticalSequenceNumberAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/UpdateGraphVerticalSequenceNumber";
        public string CreateGraphVerticalViewSequenceAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGraphMaster/CreateGraphVerticalViewSequence";
    }
}