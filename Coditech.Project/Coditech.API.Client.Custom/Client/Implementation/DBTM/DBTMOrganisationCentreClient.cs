using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Newtonsoft.Json;
using System.Net;

namespace Coditech.API.Client
{
    public class DBTMOrganisationCentreClient : BaseClient, IDBTMOrganisationCentreClient
    {
        DBTMOrganisationCentreEndpoint dBTMOrganisationCentreEndpoint = null;
        public DBTMOrganisationCentreClient()
        {
            dBTMOrganisationCentreEndpoint = new DBTMOrganisationCentreEndpoint();
        }
        public virtual DBTMActivityListViewSequenceListResponse GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ActivityListViewSequenceListAsync(dBTMOrganisationCentreMasterId, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMActivityListViewSequenceListResponse> ActivityListViewSequenceListAsync(int dBTMOrganisationCentreMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMOrganisationCentreEndpoint.GetActivityListViewSequenceListAsync(dBTMOrganisationCentreMasterId, expand, filter, sort, pageIndex, pageSize);
            HttpResponseMessage response = null;
            bool disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await GetResourceFromEndpointAsync(endpoint, status, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                Dictionary<string, IEnumerable<string>> headers = BindHeaders(response);
                switch ((int)response.StatusCode)
                {
                    case 200:
                        {
                            ObjectResponseResult<DBTMActivityListViewSequenceListResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMActivityListViewSequenceListResponse>(response, headers, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    case 204:
                        return new DBTMActivityListViewSequenceListResponse();
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMActivityListViewSequenceListResponse result = JsonConvert.DeserializeObject<DBTMActivityListViewSequenceListResponse>(value);
                            UpdateApiStatus(result, status, response);
                            throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                        }
                }
            }
            finally
            {
                if (disposeResponse)
                {
                    response.Dispose();
                }
            }
        }
        public virtual DBTMCentrewiseTestParameterListViewResponse GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId, string centreCode)
        {
            return Task.Run(async () => await GetDBTMCentrewiseTestParameterListViewAsync(dBTMOrganisationCentreParameterListViewSequenceId, centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMCentrewiseTestParameterListViewResponse> GetDBTMCentrewiseTestParameterListViewAsync(int dBTMOrganisationCentreParameterListViewSequenceId, string centreCode, CancellationToken cancellationToken)
        {
            if (dBTMOrganisationCentreParameterListViewSequenceId <= 0)
                throw new System.ArgumentNullException("DBTMOrganisationCentreParameterListViewSequenceId");

            string endpoint = dBTMOrganisationCentreEndpoint.GetDBTMCentrewiseTestParameterListViewAsync(dBTMOrganisationCentreParameterListViewSequenceId, centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentrewiseTestParameterListViewResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMCentrewiseTestParameterListViewResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMCentrewiseTestParameterListViewResponse typedBody = JsonConvert.DeserializeObject<DBTMCentrewiseTestParameterListViewResponse>(responseData);
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

        public virtual DBTMCentrewiseTestParameterListViewResponse UpdateDBTMCentrewiseTestParameterListView(DBTMCentrewiseTestParameterListViewModel body)
        {
            return Task.Run(async () => await UpdateDBTMCentrewiseTestParameterListViewAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMCentrewiseTestParameterListViewResponse> UpdateDBTMCentrewiseTestParameterListViewAsync(DBTMCentrewiseTestParameterListViewModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMOrganisationCentreEndpoint.UpdateDBTMCentrewiseTestParameterListViewAsync();
            HttpResponseMessage response = null;
            bool disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                Dictionary<string, IEnumerable<string>> dictionary = BindHeaders(response);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            ObjectResponseResult<DBTMCentrewiseTestParameterListViewResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMCentrewiseTestParameterListViewResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMCentrewiseTestParameterListViewResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMCentrewiseTestParameterListViewResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMCentrewiseTestParameterListViewResponse result = JsonConvert.DeserializeObject<DBTMCentrewiseTestParameterListViewResponse>(value);
                            UpdateApiStatus(result, status, response);
                            throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                        }
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
