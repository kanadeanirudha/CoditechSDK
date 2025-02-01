using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;

using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public partial class DBTMDashboardClient : BaseClient, IDBTMDashboardClient
    {
        DBTMDashboardEndpoint dashboardEndpoint = null;
        public DBTMDashboardClient()
        {
            dashboardEndpoint = new DBTMDashboardEndpoint();
        }

        public virtual DBTMDashboardResponse GetDBTMDashboardDetails(int selectedAdminRoleMasterId, long userMasterId)
        {
            return Task.Run(async () => await GetDBTMDashboardDetailsAsync(selectedAdminRoleMasterId, userMasterId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMDashboardResponse> GetDBTMDashboardDetailsAsync(int selectedAdminRoleMasterId, long userMasterId, System.Threading.CancellationToken cancellationToken)
        {
            if (selectedAdminRoleMasterId <= 0)
                throw new System.ArgumentNullException("selectedAdminRoleMasterId");

            string endpoint = dashboardEndpoint.GetDBTMDashboardDetailsAsync(selectedAdminRoleMasterId, userMasterId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMDashboardResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMDashboardResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMDashboardResponse typedBody = JsonConvert.DeserializeObject<DBTMDashboardResponse>(responseData);
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