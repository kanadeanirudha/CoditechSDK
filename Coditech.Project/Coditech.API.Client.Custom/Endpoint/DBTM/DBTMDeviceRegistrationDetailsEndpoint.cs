using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMDeviceRegistrationDetailsEndpoint : BaseEndpoint
    {
        public string ListAsync(long UserMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceRegistrationDetails/GetDBTMDeviceRegistrationDetailsList?UserMasterId={UserMasterId}{BuildEndpointQueryString(true, expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateRegistrationDetailsAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceRegistrationDetails/CreateRegistrationDetails";

        public string GetRegistrationDetailsAsync(long dBTMDeviceRegistrationDetailId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceRegistrationDetails/GetRegistrationDetails?dBTMDeviceRegistrationDetailId={dBTMDeviceRegistrationDetailId}";

        public string UpdateRegistrationDetailsAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceRegistrationDetails/UpdateRegistrationDetails";

        public string DeleteRegistrationDetailsAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceRegistrationDetails/DeleteRegistrationDetails";
    }
}
