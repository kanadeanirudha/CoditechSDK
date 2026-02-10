using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
namespace Coditech.API.Endpoint
{
    public class DBTMCentreWiseSettingEndpoint : BaseEndpoint
    {
        public string GetDBTMCentreWiseSettingAsync(int organisationCentreId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreWiseSetting/GetDBTMCentreWiseSetting?organisationCentreId={organisationCentreId}";

        public string UpdateDBTMCentreWiseSettingAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreWiseSetting/UpdateDBTMCentreWiseSetting";
        public string AssociateUnAssociateCentreTestAsync()
        {
            return $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMCentreWiseSetting/AssociateUnAssociateCentreTest";
        }
    }
}
