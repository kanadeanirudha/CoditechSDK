using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Endpoint
{
    public class DBTMDeviceEndpoint : BaseEndpoint
    {
        public string ListAsync(long dBTMParentDeviceMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceMaster/GetDBTMDeviceList?dBTMParentDeviceMasterId={dBTMParentDeviceMasterId}{BuildEndpointQueryString(true,expand, filter, sort, pageIndex, pageSize)}";
            return endpoint;
        }
        public string CreateDBTMDeviceAsync() =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceMaster/CreateDBTMDevice";

        public string GetDBTMDeviceAsync(long dBTMDeviceId) =>
            $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceMaster/GetDBTMDevice?dBTMDeviceId={dBTMDeviceId}";

        public string UpdateDBTMDeviceAsync() =>
               $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceMaster/UpdateDBTMDevice";

        public string DeleteDBTMDeviceAsync() =>
                  $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMDeviceMaster/DeleteDBTMDevice";
    }
}
