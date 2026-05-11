using Coditech.Admin.Utilities;
using Coditech.API.Client.Endpoint;
namespace Coditech.API.Endpoint
{
    public class DBTMGeneralCommonEndpoint : BaseEndpoint
    {
        public string GetDBTMDeviceDataDecryptedAsync(string dBTMDeviceDataIds)
        {
            string endpoint = $"{CoditechCustomAdminSettings.CoditechDBTMApiRootUri}/DBTMGeneralCommon/GetDBTMDeviceDataDecrypted?dBTMDeviceDataIds={dBTMDeviceDataIds}{BuildEndpointQueryString(true)}";
            return endpoint;
        }
    }
}
