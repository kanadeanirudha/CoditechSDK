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
    public class DBTMPrintQRClient : BaseClient, IDBTMPrintQRClient
    {
        DBTMPrintQREndpoint dBTMPrintQREndpoint = null;
        public DBTMPrintQRClient()
        {
            dBTMPrintQREndpoint = new DBTMPrintQREndpoint();
        }

        public virtual DBTMPrintQRResponse DBTMPrintQR(DBTMPrintQRModel body)
        {
            return Task.Run(async () => await DBTMPrintQRAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMPrintQRResponse> DBTMPrintQRAsync(DBTMPrintQRModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMPrintQREndpoint.DBTMPrintQRAsync();
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
                            ObjectResponseResult<DBTMPrintQRResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMPrintQRResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMPrintQRResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMPrintQRResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMPrintQRResponse result = JsonConvert.DeserializeObject<DBTMPrintQRResponse>(value);
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

        public virtual DBTMPrintQRListResponse GetDBTMPrintQR(string personIds)
        {
            return Task.Run(async () => await GetDBTMPrintQRAsync(personIds, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMPrintQRListResponse> GetDBTMPrintQRAsync(string personIds, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = dBTMPrintQREndpoint.GetDBTMPrintQRAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                ParameterModel model = new ParameterModel{ Ids = personIds };
                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(model), status, cancellationToken);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMPrintQRListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMPrintQRListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMPrintQRListResponse typedBody = JsonConvert.DeserializeObject<DBTMPrintQRListResponse>(responseData);
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

        public virtual DBTMPrintQRListResponse GetDBTMPrintQRTraineeList(int generalBatchMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await GetDBTMPrintQRTraineeListAsync(generalBatchMasterId, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMPrintQRListResponse> GetDBTMPrintQRTraineeListAsync(int generalBatchMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMPrintQREndpoint.GetDBTMPrintQRTraineeListAsync(generalBatchMasterId, expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMPrintQRListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMPrintQRListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMPrintQRListResponse typedBody = JsonConvert.DeserializeObject<DBTMPrintQRListResponse>(responseData);
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
