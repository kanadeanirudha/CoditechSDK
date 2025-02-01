using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMNewRegistrationEndpoint : BaseEndpoint
    {
        public string NewRegistrationAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreRegistration/CentreRegistration";
    }
}
