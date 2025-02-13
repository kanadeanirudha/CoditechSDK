using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;

using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public partial class CustomDashboardClient : BaseClient, ICustomDashboardClient
    {
        CustomDashboardEndpoint dashboardEndpoint = null;
        public CustomDashboardClient()
        {
            dashboardEndpoint = new CustomDashboardEndpoint();
        }

        public virtual CustomDashboardResponse GetCustomDashboardDetails(int selectedAdminRoleMasterId, long userMasterId)
        {
            return Task.Run(async () => await GetCustomDashboardDetailsAsync(selectedAdminRoleMasterId, userMasterId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<CustomDashboardResponse> GetCustomDashboardDetailsAsync(int selectedAdminRoleMasterId, long userMasterId, System.Threading.CancellationToken cancellationToken)
        {
            if (selectedAdminRoleMasterId <= 0)
                throw new System.ArgumentNullException("selectedAdminRoleMasterId");

            string endpoint = dashboardEndpoint.GetCustomDashboardDetailsAsync(selectedAdminRoleMasterId, userMasterId);
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
                    var objectResponse = await ReadObjectResponseAsync<CustomDashboardResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new CustomDashboardResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    CustomDashboardResponse typedBody = JsonConvert.DeserializeObject<CustomDashboardResponse>(responseData);
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