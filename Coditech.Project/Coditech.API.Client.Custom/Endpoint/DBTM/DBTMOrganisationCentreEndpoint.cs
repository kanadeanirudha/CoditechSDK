using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMOrganisationCentreEndpoint : BaseEndpoint
    {
        public string GetActivityListViewSequenceListAsync(int dBTMOrganisationCentreMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMOrganisationCentreMaster/GetActivityListViewSequenceList?dBTMOrganisationCentreMasterId={dBTMOrganisationCentreMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
        }
        public string GetDBTMCentrewiseTestParameterListViewAsync(int dBTMOrganisationCentreParameterListViewSequenceId, string centreCode) =>
           $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMOrganisationCentreMaster/GetDBTMCentrewiseTestParameterListView?dBTMOrganisationCentreParameterListViewSequenceId={dBTMOrganisationCentreParameterListViewSequenceId}&centreCode={centreCode}";
        public string UpdateDBTMCentrewiseTestParameterListViewAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMOrganisationCentreMaster/UpdateDBTMCentrewiseTestParameterListView";
    }
}
