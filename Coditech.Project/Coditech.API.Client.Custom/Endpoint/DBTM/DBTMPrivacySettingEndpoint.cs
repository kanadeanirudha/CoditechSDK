using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;
namespace Coditech.API.Endpoint
{
    public class DBTMPrivacySettingEndpoint : BaseEndpoint
    {
        public string ListAsync(string selectedCentreCode,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrivacySetting/GetDBTMPrivacySettingList?selectedCentreCode={selectedCentreCode}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMPrivacySettingAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrivacySetting/CreateDBTMPrivacySetting";
        public string GetDBTMPrivacySettingAsync(long dBTMPrivacySettingId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrivacySetting/GetDBTMPrivacySetting?dBTMPrivacySettingId={dBTMPrivacySettingId}";

        public string UpdateDBTMPrivacySettingAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrivacySetting/UpdateDBTMPrivacySetting";

        public string DeleteDBTMPrivacySettingAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMPrivacySetting/DeleteDBTMPrivacySetting";

    }
}
