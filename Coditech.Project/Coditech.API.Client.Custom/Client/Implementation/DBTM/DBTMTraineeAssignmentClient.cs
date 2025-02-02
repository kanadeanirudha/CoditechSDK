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
    public class DBTMTraineeAssignmentClient : BaseClient, IDBTMTraineeAssignmentClient
    {
        DBTMTraineeAssignmentEndpoint dBTMTraineeAssignmentEndpoint = null;
        public DBTMTraineeAssignmentClient()
        {
            dBTMTraineeAssignmentEndpoint = new DBTMTraineeAssignmentEndpoint();
        }

        public virtual DBTMTraineeAssignmentListResponse List(long generalTrainerMasterId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ListAsync(generalTrainerMasterId,expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeAssignmentListResponse> ListAsync(long generalTrainerMasterId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeAssignmentEndpoint.ListAsync(generalTrainerMasterId,expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeAssignmentListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTraineeAssignmentListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeAssignmentListResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeAssignmentListResponse>(responseData);
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

        public virtual DBTMTraineeAssignmentResponse CreateDBTMTraineeAssignment(DBTMTraineeAssignmentModel body)
        {
            return Task.Run(async () => await CreateDBTMTraineeAssignmentAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeAssignmentResponse> CreateDBTMTraineeAssignmentAsync(DBTMTraineeAssignmentModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeAssignmentEndpoint.CreateDBTMTraineeAssignmentAsync();
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
                            ObjectResponseResult<DBTMTraineeAssignmentResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMTraineeAssignmentResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMTraineeAssignmentResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMTraineeAssignmentResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMTraineeAssignmentResponse result = JsonConvert.DeserializeObject<DBTMTraineeAssignmentResponse>(value);
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

        public virtual DBTMTraineeAssignmentResponse GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId)
        {
            return Task.Run(async () => await GetDBTMTraineeAssignmentAsync(dBTMTraineeAssignmentId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeAssignmentResponse> GetDBTMTraineeAssignmentAsync(long dBTMTraineeAssignmentId, CancellationToken cancellationToken)
        {
            if (dBTMTraineeAssignmentId <= 0)
                throw new System.ArgumentNullException("dBTMTraineeAssignmentId");

            string endpoint = dBTMTraineeAssignmentEndpoint.GetDBTMTraineeAssignmentAsync(dBTMTraineeAssignmentId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeAssignmentResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMTraineeAssignmentResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeAssignmentResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeAssignmentResponse>(responseData);
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

        public virtual DBTMTraineeAssignmentResponse UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentModel body)
        {
            return Task.Run(async () => await UpdateDBTMTraineeAssignmentAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeAssignmentResponse> UpdateDBTMTraineeAssignmentAsync(DBTMTraineeAssignmentModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeAssignmentEndpoint.UpdateDBTMTraineeAssignmentAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeAssignmentResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeAssignmentResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeAssignmentResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeAssignmentResponse>(responseData);
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

        public virtual TrueFalseResponse DeleteDBTMTraineeAssignment(ParameterModel body)
        {
            return Task.Run(async () => await DeleteDBTMTraineeAssignmentAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteDBTMTraineeAssignmentAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeAssignmentEndpoint.DeleteDBTMTraineeAssignmentAsync();
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

        public virtual GeneralTrainerListResponse GetTrainerByCentreCode(string centreCode)
        {
            return Task.Run(async () => await GetDBTMTrainerByCentreCode(centreCode, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GeneralTrainerListResponse> GetDBTMTrainerByCentreCode(string centreCode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeAssignmentEndpoint.GetDBTMTrainerByCentreCode(centreCode);
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
                    var objectResponse = await ReadObjectResponseAsync<GeneralTrainerListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new GeneralTrainerListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    GeneralTrainerListResponse typedBody = JsonConvert.DeserializeObject<GeneralTrainerListResponse>(responseData);
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

        public virtual DBTMTraineeDetailsListResponse GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId)
        {
            return Task.Run(async () => await GetTraineeDetailsByCentreCodeAndgeneralTrainerId(centreCode, generalTrainerId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeDetailsListResponse> GetTraineeDetailsByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeAssignmentEndpoint.GetTraineeDetailsByCentreCodeAndgeneralTrainerId(centreCode, generalTrainerId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeDetailsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMTraineeDetailsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeDetailsListResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeDetailsListResponse>(responseData);
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

        public virtual TrueFalseResponse SendAssignmentReminder(string dBTMTraineeAssignmentId)
        {
            return Task.Run(async () => await SendAssignmentReminderAsync(dBTMTraineeAssignmentId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> SendAssignmentReminderAsync(string dBTMTraineeAssignmentId, CancellationToken cancellationToken)
        {

            string endpoint = dBTMTraineeAssignmentEndpoint.SendAssignmentReminderAsync(dBTMTraineeAssignmentId);
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
                    var objectResponse = await ReadObjectResponseAsync<TrueFalseResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new TrueFalseResponse();
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
