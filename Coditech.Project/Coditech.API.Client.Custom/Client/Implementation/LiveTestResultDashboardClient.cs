using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;

using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public partial class LiveTestResultDashboardClient : BaseClient, ILiveTestResultDashboardClient
    {
        LiveTestResultDashboardEndpoint liveTestResultDashboardEndpoint = null;
        public LiveTestResultDashboardClient()
        {
            liveTestResultDashboardEndpoint = new LiveTestResultDashboardEndpoint();
        }

        public virtual LiveTestResultLoginResponse GetLiveTestResultDashboard(LiveTestResultLoginModel body)
        {
            return Task.Run(async () => await GetLiveTestResultDashboardAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<LiveTestResultLoginResponse> GetLiveTestResultDashboardAsync(LiveTestResultLoginModel body, CancellationToken cancellationToken)
        {
            string endpoint = liveTestResultDashboardEndpoint.GetLiveTestResultDashboardAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);

                var headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<LiveTestResultLoginResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<LiveTestResultLoginResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    LiveTestResultLoginResponse typedBody = JsonConvert.DeserializeObject<LiveTestResultLoginResponse>(responseData);
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