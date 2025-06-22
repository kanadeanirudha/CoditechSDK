using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class LiveTestResultDashboardEndpoint : BaseEndpoint
    {
        public string GetLiveTestResultDashboardAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/LiveTestResultDashboard/GetLiveTestResultDashboard";
    }
}
