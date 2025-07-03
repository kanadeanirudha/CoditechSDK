using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;

namespace Coditech.API.Endpoint
{
    public class DBTMNewRegistrationEndpoint : BaseEndpoint
    {
        public string DBTMCentreRegistrationAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreRegistration/DBTMCentreRegistration";

        public string TrainerRegistrationAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/TrainerRegistration/TrainerRegistration";

        public string GetGeneralTrainerByJoiningCode(string joiningCode)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMUser/GetGeneralTrainerByJoiningCode?joiningCode={joiningCode}";
            return endpoint;
        }

    }
}
