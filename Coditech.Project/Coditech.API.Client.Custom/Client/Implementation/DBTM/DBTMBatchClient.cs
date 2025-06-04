using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public class DBTMBatchClient : BaseClient, IDBTMBatchClient
    {
        DBTMBatchEndpoint dBTMBatchEndpoint = null;
        public DBTMBatchClient()
        {
            dBTMBatchEndpoint = new DBTMBatchEndpoint();
        }
        
        public virtual DBTMBatchListResponse GetBatchList(long entityId, string userType)
        {
            return Task.Run(async () => await DBTMBatchAsync(entityId, userType, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMBatchListResponse> DBTMBatchAsync(long entityId, string userType, CancellationToken cancellationToken)
        {
            string endpoint = dBTMBatchEndpoint.DBTMBatchAsync(entityId, userType);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMBatchListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMBatchListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMBatchListResponse typedBody = JsonConvert.DeserializeObject<DBTMBatchListResponse>(responseData);
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
