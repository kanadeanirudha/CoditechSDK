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
        public string GetDBTMGraphAsync(int dBTMTestMasterId) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetDBTMGraph?dBTMTestMasterId={dBTMTestMasterId}";
        public string GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId, string graphMode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/DBTMGraphByDBTMTestMasterId?dBTMTestMasterId={dBTMTestMasterId}&graphMode={graphMode}";
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
        public string CreateActivityListViewSequenceAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/CreateActivityListViewSequence";

        public string GetActivityVerticalViewSequenceListAsync(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetActivityVerticalViewSequenceList?dBTMTestMasterId={dBTMTestMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
        }
        public string DeleteActivityVerticalViewSequenceAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/DeleteActivityVerticalViewSequence";

        public string GetActivityVerticalViewSequenceAsync(int dBTMTestParameterVerticalViewSequenceId) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetActivityVerticalViewSequence?dBTMTestParameterVerticalViewSequenceId={dBTMTestParameterVerticalViewSequenceId}";

        public string UpdateActivityVerticalViewSequenceAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/UpdateActivityVerticalViewSequence";
        public string UpdateVerticalSequenceNumberAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/UpdateVerticalSequenceNumber";
        public string CreateActivityVerticalViewSequenceAsync() =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/CreateActivityVerticalViewSequence";
        public string GetTestsByCentreCode(string centreCode) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMTestMaster/GetTestsByCentreCode?centreCode={centreCode}";
    }
}
