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

        public string GetDBTMTestCalculationAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMTestCalculation";
        public string GetDBTMGraphAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMGraph";
        public string GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/DBTMGraphByDBTMTestMasterId?dBTMTestMasterId={dBTMTestMasterId}";
            return endpoint;
        }
        public string GetDBTMPerformanceMatrixListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMPerformanceMatrixList{BuildEndpointQueryString(expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string GetActivityListViewSequenceListAsync(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetActivityListViewSequenceList?dBTMTestMasterId={dBTMTestMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
        }
        public string DeleteActivityListViewSequenceAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/DeleteActivityListViewSequence";

        public string GetActivityListViewSequenceAsync(int dBTMTestParameterListViewSequenceId) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetActivityListViewSequence?dBTMTestParameterListViewSequenceId={dBTMTestParameterListViewSequenceId}";

        public string UpdateActivityListViewSequenceAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/UpdateActivityListViewSequence";
        public string UpdateSequenceNumberAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/UpdateSequenceNumber";
    }
}
