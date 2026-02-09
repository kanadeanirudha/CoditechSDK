using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Newtonsoft.Json;
namespace Coditech.API.Client
{
    public class DBTMCentreWiseSettingClient : BaseClient, IDBTMCentreWiseSettingClient
    {
        DBTMCentreWiseSettingEndpoint dBTMCentreWiseSettingEndpoint = null;
        public DBTMCentreWiseSettingClient()
        {
            dBTMCentreWiseSettingEndpoint = new DBTMCentreWiseSettingEndpoint();
        }

        public virtual DBTMCentreWiseSettingResponse GetDBTMCentreWiseSetting(int organisationCentreId)
        {
            return Task.Run(async () => await GetDBTMCentreWiseSettingAsync(organisationCentreId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMCentreWiseSettingResponse> GetDBTMCentreWiseSettingAsync(int organisationCentreId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMCentreWiseSettingEndpoint.GetDBTMCentreWiseSettingAsync(organisationCentreId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentreWiseSettingResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMCentreWiseSettingResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMCentreWiseSettingResponse typedBody = JsonConvert.DeserializeObject<DBTMCentreWiseSettingResponse>(responseData);
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
        public virtual DBTMCentreWiseTestResponse AssociateUnAssociateCentreTest(DBTMCentreWiseTestModel body)
        {
            return Task.Run(async () => await AssociateUnAssociateCentreTestAsync(body, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMCentreWiseTestResponse> AssociateUnAssociateCentreTestAsync(DBTMCentreWiseTestModel body,System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = dBTMCentreWiseSettingEndpoint.AssociateUnAssociateCentreTestAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await PutResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);
                var headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentreWiseTestResponse>( response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentreWiseTestResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException( objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMCentreWiseTestResponse typedBody = JsonConvert.DeserializeObject<DBTMCentreWiseTestResponse>(responseData);
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
        public virtual DBTMCentreWiseSettingResponse UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingModel body)
        {
            return Task.Run(async () => await UpdateDBTMCentreWiseSettingAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMCentreWiseSettingResponse> UpdateDBTMCentreWiseSettingAsync(DBTMCentreWiseSettingModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMCentreWiseSettingEndpoint.UpdateDBTMCentreWiseSettingAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();

                response = await PutResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);

                var headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentreWiseSettingResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentreWiseSettingResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMCentreWiseSettingResponse typedBody = JsonConvert.DeserializeObject<DBTMCentreWiseSettingResponse>(responseData);
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


