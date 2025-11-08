using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.API.Model;

using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public class DBTMReportsClient : BaseClient, IDBTMReportsClient
    {
        DBTMReportsEndpoint dBTMReportsEndpoint = null;
        public DBTMReportsClient()
        {
            dBTMReportsEndpoint = new DBTMReportsEndpoint();
        }

        public virtual DBTMTestWiseReportsListResponse BatchWiseMultipleReports(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate)
        {
            return Task.Run(async () => await BatchWiseMultipleReportsAsync(dBTMTestMasterIds, generalBatchMasterId, fromDate, toDate, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestWiseReportsListResponse> BatchWiseMultipleReportsAsync(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.BatchWiseMultipleReportsAsync(dBTMTestMasterIds, generalBatchMasterId, fromDate, toDate);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWiseReportsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWiseReportsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWiseReportsListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWiseReportsListResponse>(responseData);
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

        public virtual DBTMTestWiseReportsListResponse BatchWiseMultipleReportsFile(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType)
        {
            return Task.Run(async () => await BatchWiseMultipleReportsFileAsync(dBTMTestMasterIds, generalBatchMasterId, fromDate, toDate, entityId, userType, centreCode, reportType, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestWiseReportsListResponse> BatchWiseMultipleReportsFileAsync(string dBTMTestMasterIds, int generalBatchMasterId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.BatchWiseMultipleReportsFileAsync(dBTMTestMasterIds, generalBatchMasterId, fromDate, toDate, entityId, userType, centreCode, reportType);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWiseReportsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWiseReportsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWiseReportsListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWiseReportsListResponse>(responseData);
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

        public virtual DBTMTestWiseReportsListResponse TestWiseMultipleReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            return Task.Run(async () => await TestWiseMultipleReportsAsync(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestWiseReportsListResponse> TestWiseMultipleReportsAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.TestWiseMultipleReportsAsync(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWiseReportsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWiseReportsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWiseReportsListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWiseReportsListResponse>(responseData);
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

        public virtual DBTMTestWiseReportsListResponse TestWiseMultipleReportsFile(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType)
        {
            return Task.Run(async () => await TestWiseMultipleReportsFileAsync(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, reportType, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestWiseReportsListResponse> TestWiseMultipleReportsFileAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, string reportType, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.TestWiseMultipleReportsFileAsync(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, reportType);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWiseReportsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWiseReportsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWiseReportsListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWiseReportsListResponse>(responseData);
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

        public virtual GraphResponse TestWiseGraphReports(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            return Task.Run(async () => await TestWiseGraphReportsAsync(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, graphMode, fromDate, toDate, entityId, userType, centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GraphResponse> TestWiseGraphReportsAsync(int dBTMTestMasterId, long dBTMTraineeDetailId, int dBTMGraphMasterId, string graphMode, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.TestWiseGraphReportsAsync(dBTMTestMasterId, dBTMTraineeDetailId, dBTMGraphMasterId, graphMode, fromDate, toDate, entityId, userType, centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<GraphResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GraphResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GraphResponse typedBody = JsonConvert.DeserializeObject<GraphResponse>(responseData);
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

        public virtual DBTMTestWiseReportsListResponse NameWiseReports(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode)
        {
            return Task.Run(async () => await NameWiseReportsAsync(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestWiseReportsListResponse> NameWiseReportsAsync(string dBTMTestMasterIds, long dBTMTraineeDetailId, DateTime fromDate, DateTime toDate, long entityId, string userType, string centreCode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.NameWiseReportsAsync(dBTMTestMasterIds, dBTMTraineeDetailId, fromDate, toDate, entityId, userType, centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTestWiseReportsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTestWiseReportsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTestWiseReportsListResponse typedBody = JsonConvert.DeserializeObject<DBTMTestWiseReportsListResponse>(responseData);
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

        public virtual TrueFalseResponse DeleteReportsFile(ParameterModel body)
        {
            return Task.Run(async () => await DeleteReportsFileAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteReportsFileAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.DeleteReportsFileAsync();
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
