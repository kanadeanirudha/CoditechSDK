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
    public class DBTMOrganisationCentrewiseJoiningCodeClient : BaseClient, IDBTMOrganisationCentrewiseJoiningCodeClient
    {
        DBTMOrganisationCentrewiseJoiningCodeEndpoint dBTMOrganisationCentrewiseJoiningCodeEndpoint = null;
        public DBTMOrganisationCentrewiseJoiningCodeClient()
        {
            dBTMOrganisationCentrewiseJoiningCodeEndpoint = new DBTMOrganisationCentrewiseJoiningCodeEndpoint();
        }

        public virtual DBTMOrganisationCentrewiseJoiningCodeResponse GetTraineeActiveJoiningCode(string centreCode, string trainerId)
        {
            return Task.Run(async () => await GetTraineeActiveJoiningCodeAsync(centreCode, trainerId, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMOrganisationCentrewiseJoiningCodeResponse> GetTraineeActiveJoiningCodeAsync(string centreCode, string trainerId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMOrganisationCentrewiseJoiningCodeEndpoint.GetTraineeActiveJoiningCodeAsync(centreCode, trainerId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMOrganisationCentrewiseJoiningCodeResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMOrganisationCentrewiseJoiningCodeResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMOrganisationCentrewiseJoiningCodeResponse typedBody = JsonConvert.DeserializeObject<DBTMOrganisationCentrewiseJoiningCodeResponse>(responseData);
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

        public virtual DBTMOrganisationCentrewiseJoiningCodeResponse GetTrainerActiveJoiningCode(string centreCode)
        {
            return Task.Run(async () => await GetTrainerActiveJoiningCodeAsync(centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMOrganisationCentrewiseJoiningCodeResponse> GetTrainerActiveJoiningCodeAsync(string centreCode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMOrganisationCentrewiseJoiningCodeEndpoint.GetTrainerActiveJoiningCodeAsync(centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMOrganisationCentrewiseJoiningCodeResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMOrganisationCentrewiseJoiningCodeResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMOrganisationCentrewiseJoiningCodeResponse typedBody = JsonConvert.DeserializeObject<DBTMOrganisationCentrewiseJoiningCodeResponse>(responseData);
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

        public virtual OrganisationCentrewiseJoiningCodeListResponse GetTraineeActiveJoiningCodeList(string centreCode, string trainerId, int rows)
        {
            return Task.Run(async () => await GetTraineeActiveJoiningCodeListAsync(centreCode, trainerId, rows, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<OrganisationCentrewiseJoiningCodeListResponse> GetTraineeActiveJoiningCodeListAsync(string centreCode, string trainerId, int rows, CancellationToken cancellationToken)
        {
            string endpoint = dBTMOrganisationCentrewiseJoiningCodeEndpoint.GetTraineeActiveJoiningCodeListAsync(centreCode, trainerId, rows);
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await GetResourceFromEndpointAsync(endpoint, status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var statusCode = (int)response.StatusCode;
                if (statusCode == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<OrganisationCentrewiseJoiningCodeListResponse>( response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(status.ErrorCode, status.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (statusCode == 204)
                {
                    return new OrganisationCentrewiseJoiningCodeListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var typedBody = JsonConvert.DeserializeObject<OrganisationCentrewiseJoiningCodeListResponse>(responseData);
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

        public virtual TrueFalseResponse DeleteJoiningCodeFile(ParameterModel body)
        {
            return Task.Run(async () => await DeleteJoiningCodeFileAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<TrueFalseResponse> DeleteJoiningCodeFileAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMOrganisationCentrewiseJoiningCodeEndpoint.DeleteJoiningCodeFileAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<TrueFalseResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    TrueFalseResponse typedBody = JsonConvert.DeserializeObject<TrueFalseResponse>(responseData);
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
