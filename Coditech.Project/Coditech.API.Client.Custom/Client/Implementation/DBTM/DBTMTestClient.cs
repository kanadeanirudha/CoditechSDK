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
    public class DBTMTestClient : BaseClient, IDBTMTestClient
    {
        DBTMTestEndpoint dBTMTestEndpoint = null;
        public DBTMTestClient()
        {
            dBTMTestEndpoint = new DBTMTestEndpoint();
        }

        public virtual DBTMTestListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ListAsync(expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestListResponse> ListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.ListAsync(expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestListResponse>(responseData);
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

        public virtual DBTMTestResponse CreateDBTMTest(DBTMTestModel body)
        {
            return Task.Run(async () => await CreateDBTMTestAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestResponse> CreateDBTMTestAsync(DBTMTestModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.CreateDBTMTestAsync();
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
                            ObjectResponseResult<DBTMTestResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMTestResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMTestResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMTestResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMTestResponse result = JsonConvert.DeserializeObject<DBTMTestResponse>(value);
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

        public virtual DBTMTestResponse GetDBTMTest(int dBTMTestMasterId)
        {
            return Task.Run(async () => await GetDBTMTestAsync(dBTMTestMasterId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestResponse> GetDBTMTestAsync(int dBTMTestMasterId, CancellationToken cancellationToken)
        {
            if (dBTMTestMasterId <= 0)
                throw new System.ArgumentNullException("dBTMTestMasterId");

            string endpoint = dBTMTestEndpoint.GetDBTMTestAsync(dBTMTestMasterId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMTestResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestResponse typedBody = JsonConvert.DeserializeObject<DBTMTestResponse>(responseData);
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

        public virtual DBTMTestResponse UpdateDBTMTest(DBTMTestModel body)
        {
            return Task.Run(async () => await UpdateDBTMTestAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestResponse> UpdateDBTMTestAsync(DBTMTestModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateDBTMTestAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestResponse typedBody = JsonConvert.DeserializeObject<DBTMTestResponse>(responseData);
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

        public virtual DBTMActivityListViewSequenceResponse GetActivityListViewSequence(int dBTMTestParameterListViewSequenceId)
        {
            return Task.Run(async () => await GetActivityListViewSequenceAsync(dBTMTestParameterListViewSequenceId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityListViewSequenceResponse> GetActivityListViewSequenceAsync(int dBTMTestParameterListViewSequenceId, CancellationToken cancellationToken)
        {
            if (dBTMTestParameterListViewSequenceId <= 0)
                throw new System.ArgumentNullException("DBTMTestParameterListViewSequenceId");

            string endpoint = dBTMTestEndpoint.GetActivityListViewSequenceAsync(dBTMTestParameterListViewSequenceId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMActivityListViewSequenceResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMActivityListViewSequenceResponse typedBody = JsonConvert.DeserializeObject<DBTMActivityListViewSequenceResponse>(responseData);
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

        public virtual DBTMActivityListViewSequenceResponse UpdateActivityListViewSequence(DBTMActivityListViewSequenceModel body)
        {
            return Task.Run(async () => await UpdateActivityListViewSequenceAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityListViewSequenceResponse> UpdateActivityListViewSequenceAsync(DBTMActivityListViewSequenceModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateActivityListViewSequenceAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMActivityListViewSequenceResponse typedBody = JsonConvert.DeserializeObject<DBTMActivityListViewSequenceResponse>(responseData);
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

        public virtual TrueFalseResponse DeleteDBTMTest(ParameterModel body)
        {
            return Task.Run(async () => await DeleteDBTMTestAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteDBTMTestAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.DeleteDBTMTestAsync();
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

        public virtual DBTMGraphMasterListResponse GetDBTMGraph(int dBTMTestMasterId)
        {
            return Task.Run(async () => await GetDBTMGraphAsync(dBTMTestMasterId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMGraphMasterListResponse> GetDBTMGraphAsync(int dBTMTestMasterId, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetDBTMGraphAsync(dBTMTestMasterId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMGraphMasterListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMGraphMasterListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMGraphMasterListResponse typedBody = JsonConvert.DeserializeObject<DBTMGraphMasterListResponse>(responseData);
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
        public virtual DBTMGraphMasterListResponse DBTMGraphByDBTMTestMasterId(int dBTMTestMasterId, string graphMode)
        {
            return Task.Run(async () => await GetDBTMGraphByDBTMTestMasterId(dBTMTestMasterId, graphMode, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMGraphMasterListResponse> GetDBTMGraphByDBTMTestMasterId(int dBTMTestMasterId, string graphMode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetDBTMGraphByDBTMTestMasterId(dBTMTestMasterId, graphMode);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMGraphMasterListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMGraphMasterListResponse();
                }
                else
                {
                    string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                    DBTMGraphMasterListResponse result = JsonConvert.DeserializeObject<DBTMGraphMasterListResponse>(value);
                    UpdateApiStatus(result, status, response);
                    throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                }
            }
            finally
            {
                if (disposeResponse)
                    response.Dispose();
            }
        }
        public virtual DBTMPerformanceMatrixListResponse GetDBTMPerformanceMatrixList(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await GetDBTMPerformanceMatrixListAsync(expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMPerformanceMatrixListResponse> GetDBTMPerformanceMatrixListAsync(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetDBTMPerformanceMatrixListAsync(expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMPerformanceMatrixListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMPerformanceMatrixListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMPerformanceMatrixListResponse typedBody = JsonConvert.DeserializeObject<DBTMPerformanceMatrixListResponse>(responseData);
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

        public virtual DBTMActivityListViewSequenceListResponse GetActivityListViewSequenceList(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ActivityListViewSequenceListAsync(dBTMTestMasterId, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityListViewSequenceListResponse> ActivityListViewSequenceListAsync(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetActivityListViewSequenceListAsync(dBTMTestMasterId, expand, filter, sort, pageIndex, pageSize);
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

        public virtual DBTMActivityListViewSequenceResponse UpdateSequenceNumber(DBTMActivityListViewSequenceModel body)
        {
            return Task.Run(async () => await UpdateSequenceNumberAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityListViewSequenceResponse> UpdateSequenceNumberAsync(DBTMActivityListViewSequenceModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateSequenceNumberAsync();
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
                            ObjectResponseResult<DBTMActivityListViewSequenceResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMActivityListViewSequenceResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMActivityListViewSequenceResponse result = JsonConvert.DeserializeObject<DBTMActivityListViewSequenceResponse>(value);
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

        public virtual DBTMActivityListViewSequenceResponse CreateActivityListViewSequence(DBTMActivityListViewSequenceModel body)
        {
            return Task.Run(async () => await CreateActivityListViewSequenceAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityListViewSequenceResponse> CreateActivityListViewSequenceAsync(DBTMActivityListViewSequenceModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.CreateActivityListViewSequenceAsync();
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
                            ObjectResponseResult<DBTMActivityListViewSequenceResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMActivityListViewSequenceResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMActivityListViewSequenceResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMActivityListViewSequenceResponse result = JsonConvert.DeserializeObject<DBTMActivityListViewSequenceResponse>(value);
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
        public virtual TrueFalseResponse DeleteActivityListViewSequence(ParameterModel body)
        {
            return Task.Run(async () => await DeleteActivityListViewSequenceAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteActivityListViewSequenceAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.DeleteActivityListViewSequenceAsync();
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


        public virtual DBTMActivityVerticalViewSequenceListResponse GetActivityVerticalViewSequenceList(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ActivityVerticalViewSequenceListAsync(dBTMTestMasterId, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityVerticalViewSequenceListResponse> ActivityVerticalViewSequenceListAsync(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetActivityVerticalViewSequenceListAsync(dBTMTestMasterId, expand, filter, sort, pageIndex, pageSize);
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
                            ObjectResponseResult<DBTMActivityVerticalViewSequenceListResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceListResponse>(response, headers, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    case 204:
                        return new DBTMActivityVerticalViewSequenceListResponse();
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMActivityVerticalViewSequenceListResponse result = JsonConvert.DeserializeObject<DBTMActivityVerticalViewSequenceListResponse>(value);
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

        public virtual DBTMActivityVerticalViewSequenceResponse UpdateVerticalSequenceNumber(DBTMActivityVerticalViewSequenceModel body)
        {
            return Task.Run(async () => await UpdateVerticalSequenceNumberAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMActivityVerticalViewSequenceResponse> UpdateVerticalSequenceNumberAsync(DBTMActivityVerticalViewSequenceModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateVerticalSequenceNumberAsync();
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
                            ObjectResponseResult<DBTMActivityVerticalViewSequenceResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMActivityVerticalViewSequenceResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMActivityVerticalViewSequenceResponse result = JsonConvert.DeserializeObject<DBTMActivityVerticalViewSequenceResponse>(value);
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

        public virtual DBTMActivityVerticalViewSequenceResponse CreateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel body)
        {
            return Task.Run(async () => await CreateActivityVerticalViewSequenceAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityVerticalViewSequenceResponse> CreateActivityVerticalViewSequenceAsync(DBTMActivityVerticalViewSequenceModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.CreateActivityVerticalViewSequenceAsync();
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
                            ObjectResponseResult<DBTMActivityVerticalViewSequenceResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMActivityVerticalViewSequenceResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMActivityVerticalViewSequenceResponse result = JsonConvert.DeserializeObject<DBTMActivityVerticalViewSequenceResponse>(value);
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
        public virtual TrueFalseResponse DeleteActivityVerticalViewSequence(ParameterModel body)
        {
            return Task.Run(async () => await DeleteActivityVerticalViewSequenceAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteActivityVerticalViewSequenceAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.DeleteActivityVerticalViewSequenceAsync();
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

        public virtual DBTMActivityVerticalViewSequenceResponse GetActivityVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId)
        {
            return Task.Run(async () => await GetActivityVerticalViewSequenceAsync(dBTMTestParameterVerticalViewSequenceId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityVerticalViewSequenceResponse> GetActivityVerticalViewSequenceAsync(int dBTMTestParameterVerticalViewSequenceId, CancellationToken cancellationToken)
        {
            if (dBTMTestParameterVerticalViewSequenceId <= 0)
                throw new System.ArgumentNullException("DBTMTestParameterVerticalViewSequenceId");

            string endpoint = dBTMTestEndpoint.GetActivityVerticalViewSequenceAsync(dBTMTestParameterVerticalViewSequenceId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMActivityVerticalViewSequenceResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMActivityVerticalViewSequenceResponse typedBody = JsonConvert.DeserializeObject<DBTMActivityVerticalViewSequenceResponse>(responseData);
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

        public virtual DBTMActivityVerticalViewSequenceResponse UpdateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel body)
        {
            return Task.Run(async () => await UpdateActivityVerticalViewSequenceAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivityVerticalViewSequenceResponse> UpdateActivityVerticalViewSequenceAsync(DBTMActivityVerticalViewSequenceModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateActivityVerticalViewSequenceAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivityVerticalViewSequenceResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMActivityVerticalViewSequenceResponse typedBody = JsonConvert.DeserializeObject<DBTMActivityVerticalViewSequenceResponse>(responseData);
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
        public virtual DBTMCentreWiseTestListResponse GetTestsByCentreCode(string centreCode)
        {
            return Task.Run(async () => await GetTestsByCentreCodeAsync(centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMCentreWiseTestListResponse> GetTestsByCentreCodeAsync(string centreCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(centreCode))
                throw new ArgumentNullException(nameof(centreCode));
            string endpoint = dBTMTestEndpoint.GetTestsByCentreCode(centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMCentreWiseTestListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMCentreWiseTestListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMCentreWiseTestListResponse typedBody = JsonConvert.DeserializeObject<DBTMCentreWiseTestListResponse>(responseData);
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

        public virtual DBTMTestWisePerformanceStandardListResponse GetDBTMTestWisePerformanceStandardList(int dBTMTestMasterId, short dBTMTestwisePerformanceStandardCategoryId)
        {
            return Task.Run(async () => await GetDBTMTestWisePerformanceStandardListAsync(dBTMTestMasterId, dBTMTestwisePerformanceStandardCategoryId, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMTestWisePerformanceStandardListResponse> GetDBTMTestWisePerformanceStandardListAsync(int dBTMTestMasterId, short dBTMTestwisePerformanceStandardCategoryId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetDBTMTestWisePerformanceStandardListAsync(dBTMTestMasterId, dBTMTestwisePerformanceStandardCategoryId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWisePerformanceStandardListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWisePerformanceStandardListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWisePerformanceStandardListResponse>(responseData);
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
        public virtual DBTMTestWisePerformanceStandardResponse CreateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardModel body)
        {
            return Task.Run(async () => await CreateDBTMTestWisePerformanceStandardAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMTestWisePerformanceStandardResponse> CreateDBTMTestWisePerformanceStandardAsync(DBTMTestWisePerformanceStandardModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.CreateDBTMTestWisePerformanceStandardAsync();
            HttpResponseMessage response = null;
            bool disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> dictionary = BindHeaders(response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            ObjectResponseResult<DBTMTestWisePerformanceStandardResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }
                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMTestWisePerformanceStandardResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardResponse>(response, dictionary, cancellationToken).ConfigureAwait(false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }
                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = response.Content != null ? await response.Content.ReadAsStringAsync().ConfigureAwait(false) : null;
                            DBTMTestWisePerformanceStandardResponse result = JsonConvert.DeserializeObject<DBTMTestWisePerformanceStandardResponse>(value);
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

        public virtual DBTMTestWisePerformanceStandardResponse UpdateDBTMTestWisePerformanceStandard(DBTMTestWisePerformanceStandardModel body)
        {
            return Task.Run(async () => await UpdateDBTMTestWisePerformanceStandardAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMTestWisePerformanceStandardResponse> UpdateDBTMTestWisePerformanceStandardAsync(DBTMTestWisePerformanceStandardModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateDBTMTestWisePerformanceStandardAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWisePerformanceStandardResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWisePerformanceStandardResponse>(responseData);
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
        public virtual DBTMTestwisePerformanceStandardCategoryListResponse GetDBTMTestwisePerformanceStandardCategoryList(short dBTMTestwisePerformanceStandardCategoryId)
        {
            return Task.Run(async () => await GetDBTMTestwisePerformanceStandardCategoryListAsync(dBTMTestwisePerformanceStandardCategoryId, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMTestwisePerformanceStandardCategoryListResponse> GetDBTMTestwisePerformanceStandardCategoryListAsync(short dBTMTestwisePerformanceStandardCategoryId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetDBTMTestwisePerformanceStandardCategoryListAsync(dBTMTestwisePerformanceStandardCategoryId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestwisePerformanceStandardCategoryListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestwisePerformanceStandardCategoryListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestwisePerformanceStandardCategoryListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestwisePerformanceStandardCategoryListResponse>(responseData);
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
        public virtual DBTMTestWisePerformanceStandardConfigurationListResponse GetDBTMTestWisePerformanceStandardConfigurationList(int dBTMTestMasterId, short dBTMTestwisePerformanceStandardCategoryId)
        {
            return Task.Run(async () => await GetDBTMTestWisePerformanceStandardConfigurationListAsync(dBTMTestMasterId, dBTMTestwisePerformanceStandardCategoryId, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMTestWisePerformanceStandardConfigurationListResponse> GetDBTMTestWisePerformanceStandardConfigurationListAsync(int dBTMTestMasterId, short dBTMTestwisePerformanceStandardCategoryId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.GetDBTMTestWisePerformanceStandardConfigurationListAsync(dBTMTestMasterId, dBTMTestwisePerformanceStandardCategoryId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardConfigurationListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWisePerformanceStandardConfigurationListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWisePerformanceStandardConfigurationListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWisePerformanceStandardConfigurationListResponse>(responseData);
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
        public virtual DBTMTestWisePerformanceStandardConfigurationResponse UpdateDBTMTestWisePerformanceStandardConfiguration(DBTMTestWisePerformanceStandardConfigurationModel body)
        {
            return Task.Run(async () => await UpdateDBTMTestWisePerformanceStandardConfigurationAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<DBTMTestWisePerformanceStandardConfigurationResponse> UpdateDBTMTestWisePerformanceStandardConfigurationAsync(DBTMTestWisePerformanceStandardConfigurationModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTestEndpoint.UpdateDBTMTestWisePerformanceStandardConfigurationAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                response = await PutResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardConfigurationResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWisePerformanceStandardConfigurationResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWisePerformanceStandardConfigurationResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWisePerformanceStandardConfigurationResponse>(responseData);
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
