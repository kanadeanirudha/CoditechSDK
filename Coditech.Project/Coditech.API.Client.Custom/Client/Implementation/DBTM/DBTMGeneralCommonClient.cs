using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Newtonsoft.Json;
namespace Coditech.API.Client
{
    public class DBTMGeneralCommonClient : BaseClient, IDBTMGeneralCommonClient
    {
        DBTMGeneralCommonEndpoint dBTMGeneralCommonEndpoint = null;
        public DBTMGeneralCommonClient()
        {
            dBTMGeneralCommonEndpoint = new DBTMGeneralCommonEndpoint();
        }
        public virtual DBTMDeviceDataDetailsResponse GetDBTMDeviceDataDecrypted(string dBTMDeviceDataIds)
        {
            return Task.Run(async () => await GetDBTMDeviceDataDecryptedAsync(dBTMDeviceDataIds, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMDeviceDataDetailsResponse> GetDBTMDeviceDataDecryptedAsync(string dBTMDeviceDataIds, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = dBTMGeneralCommonEndpoint.GetDBTMDeviceDataDecryptedAsync(dBTMDeviceDataIds);
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await GetResourceFromEndpointAsync(endpoint, status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMDeviceDataDetailsResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                    if (status_ == 204)
                    {
                        return new DBTMDeviceDataDetailsResponse();
                    }
                    else
                    {
                        string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        DBTMDeviceDataDetailsResponse typedBody = JsonConvert.DeserializeObject<DBTMDeviceDataDetailsResponse>(responseData);
                        UpdateApiStatus(typedBody, status, response);
                        throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                    }
            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }
    }
}
