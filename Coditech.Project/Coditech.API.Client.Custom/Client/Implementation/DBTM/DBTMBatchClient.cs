using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public class DBTMBatchClient : BaseClient, IDBTMBatchClient
    {
        DBTMBatchEndpoint dBTMBatchEndpoint = null;
        public DBTMBatchClient()
        {
            dBTMBatchEndpoint = new DBTMBatchEndpoint();
        }
        
        public virtual DBTMBatchListResponse GetBatchList(long entityId, string userType)
        {
            return Task.Run(async () => await DBTMBatchAsync(entityId, userType, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMBatchListResponse> DBTMBatchAsync(long entityId, string userType, CancellationToken cancellationToken)
        {
            string endpoint = dBTMBatchEndpoint.DBTMBatchAsync(entityId, userType);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMBatchListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMBatchListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMBatchListResponse typedBody = JsonConvert.DeserializeObject<DBTMBatchListResponse>(responseData);
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

        public virtual GeneralBatchUserListResponse GetDBTMBatchUserList(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId)
        {
            return Task.Run(async () => await GetDBTMBatchUserListAsync(selectedCentreCode, generalTrainerMasterId, generalBatchMasterId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GeneralBatchUserListResponse> GetDBTMBatchUserListAsync(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMBatchEndpoint.GetDBTMBatchUserListAsync(selectedCentreCode, generalTrainerMasterId, generalBatchMasterId);
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
                    var objectResponse = await ReadObjectResponseAsync<GeneralBatchUserListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GeneralBatchUserListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GeneralBatchUserListResponse typedBody = JsonConvert.DeserializeObject<GeneralBatchUserListResponse>(responseData);
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

        public virtual GeneralBatchListResponse GetCalendarBatches(string centreCode, long userMasterId, DateTime startDate, DateTime endDate)
        {
            return Task.Run(async () => await GetCalendarBatchesAsync(centreCode, userMasterId, startDate, endDate, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GeneralBatchListResponse> GetCalendarBatchesAsync(string centreCode, long userMasterId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            string endpoint = dBTMBatchEndpoint.GetCalendarBatchesAsync(centreCode, userMasterId, startDate, endDate);
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
                    var objectResponse = await ReadObjectResponseAsync<GeneralBatchListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GeneralBatchListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GeneralBatchListResponse typedBody = JsonConvert.DeserializeObject<GeneralBatchListResponse>(responseData);
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
        public virtual DBTMBatchListResponse GetDBTMCentrAndTrainerewiseBatchList(string centreCode, int joiningCodeTypeEnumId, long generalTrainerMasterId)
        {
            return Task.Run(async () => await GetDBTMCentrAndTrainerewiseBatchListAsync(centreCode, joiningCodeTypeEnumId, generalTrainerMasterId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMBatchListResponse> GetDBTMCentrAndTrainerewiseBatchListAsync(string centreCode, int joiningCodeTypeEnumId, long generalTrainerMasterId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMBatchEndpoint.GetDBTMCentrAndTrainerewiseBatchListAsync(centreCode, joiningCodeTypeEnumId, generalTrainerMasterId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMBatchListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMBatchListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMBatchListResponse typedBody = JsonConvert.DeserializeObject<DBTMBatchListResponse>(responseData);
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
        public virtual TrueFalseResponse TransferBatch(int generalBatchMasterId, long trainerId)
        {
            return Task.Run(async () => await TransferBatchAsync(generalBatchMasterId, trainerId, CancellationToken.None)).GetAwaiter().GetResult();
        }
        public virtual async Task<TrueFalseResponse> TransferBatchAsync(int generalBatchMasterId, long trainerId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMBatchEndpoint.TransferBatchAsync();
            HttpResponseMessage response = null;
            var disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                ParameterModel parameterModel = new ParameterModel
                {
                    Ids = $"{generalBatchMasterId},{trainerId}"
                };
                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(parameterModel), status, cancellationToken).ConfigureAwait(false);
                var headers_ = BindHeaders(response);
                var status_ = (int)response.StatusCode;
                if (status_ == 200)
                {
                    var objectResponse = await ReadObjectResponseAsync<TrueFalseResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                        throw new CoditechException(ErrorCodes.NullModel, "ConvertCampUserToBatchUser returned empty response.");
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
                    response?.Dispose();
            }
        }
    }
}
