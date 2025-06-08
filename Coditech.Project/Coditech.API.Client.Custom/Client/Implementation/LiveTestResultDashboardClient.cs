using Coditech.API.Endpoint;
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

        public virtual LiveTestResultDashboardResponse GetLiveTestResultDashboard(string selectedCentreCode, long entityId)
        {
            return Task.Run(async () => await GetLiveTestResultDashboardAsync(selectedCentreCode,entityId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<LiveTestResultDashboardResponse> GetLiveTestResultDashboardAsync(string selectedCentreCode, long entityId, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = liveTestResultDashboardEndpoint.GetLiveTestResultDashboardAsync( selectedCentreCode,entityId);
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
                    var objectResponse = await ReadObjectResponseAsync<LiveTestResultDashboardResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new LiveTestResultDashboardResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    LiveTestResultDashboardResponse typedBody = JsonConvert.DeserializeObject<LiveTestResultDashboardResponse>(responseData);
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