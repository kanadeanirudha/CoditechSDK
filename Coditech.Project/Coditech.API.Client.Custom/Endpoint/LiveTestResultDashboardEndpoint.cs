using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class LiveTestResultDashboardEndpoint : BaseEndpoint
    {
        public string GetLiveTestResultDashboardAsync(string selectedCentreCode, long entityId) =>
            $"{CoditechCustomAdminSettings.CoditechOrganisationApiRootUri}/LiveTestResultDashboard/GetLiveTestResultDashboard?selectedCentreCode={selectedCentreCode}&entityId={entityId}";
    }
}
