using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Net;
namespace Coditech.API.Client
{
    public class DBTMTraineeDetailsClient : BaseClient, IDBTMTraineeDetailsClient
    {
        DBTMTraineeDetailsEndpoint dBTMTraineeDetailsEndpoint = null;

        public DBTMTraineeDetailsClient()
        {
            dBTMTraineeDetailsEndpoint = new DBTMTraineeDetailsEndpoint();
        }
        public virtual DBTMTraineeDetailsListResponse List(string selectedCentreCode, long generalTrainerMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await ListAsync(selectedCentreCode, generalTrainerMasterId, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeDetailsListResponse> ListAsync(string selectedCentreCode, long generalTrainerMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeDetailsEndpoint.ListAsync(selectedCentreCode, generalTrainerMasterId, expand, filter, sort, pageIndex, pageSize);
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

        public virtual DBTMTraineeDetailsResponse GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId)
        {
            return Task.Run(async () => await GetDBTMTraineeOtherDetailsAsync(dBTMTraineeDetailId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeDetailsResponse> GetDBTMTraineeOtherDetailsAsync(long dBTMTraineeDetailId, CancellationToken cancellationToken)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new System.ArgumentNullException("dBTMTraineeDetailId");

            string endpoint = dBTMTraineeDetailsEndpoint.GetDBTMTraineeOtherDetailsAsync(dBTMTraineeDetailId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeDetailsResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMTraineeDetailsResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeDetailsResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeDetailsResponse>(responseData);
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

        public virtual DBTMTraineeDetailsResponse UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsModel body)
        {
            return Task.Run(async () => await UpdateDBTMTraineeOtherDetailsAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeDetailsResponse> UpdateDBTMTraineeOtherDetailsAsync(DBTMTraineeDetailsModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeDetailsEndpoint.UpdateDBTMTraineeOtherDetailsAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeDetailsResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeDetailsResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeDetailsResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeDetailsResponse>(responseData);
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

        public virtual TrueFalseResponse DeleteDBTMTraineeDetails(ParameterModel body)
        {
            return Task.Run(async () => await DeleteDBTMTraineeDetailsAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<TrueFalseResponse> DeleteDBTMTraineeDetailsAsync(ParameterModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeDetailsEndpoint.DeleteDBTMTraineeDetailsAsync();
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

        public virtual DBTMActivitiesListResponse GetTraineeActivitiesList(string personCode, int numberOfDaysRecord, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await GetTraineeActivitiesListAsync(personCode, numberOfDaysRecord, expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivitiesListResponse> GetTraineeActivitiesListAsync(string personCode, int numberOfDaysRecord, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeDetailsEndpoint.GetTraineeActivitiesListAsync(personCode, numberOfDaysRecord, expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivitiesListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMActivitiesListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMActivitiesListResponse typedBody = JsonConvert.DeserializeObject<DBTMActivitiesListResponse>(responseData);
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

        public virtual DBTMActivitiesDetailsListResponse GetTraineeActivitiesDetailsList(long dBTMDeviceDataId, long entityId, string userType, string centreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await GetTraineeActivitiesDetailsListAsync(dBTMDeviceDataId, entityId, userType, centreCode, expand, filter, sort, pageIndex, pageSize, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMActivitiesDetailsListResponse> GetTraineeActivitiesDetailsListAsync(long dBTMDeviceDataId, long entityId, string userType, string centreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMTraineeDetailsEndpoint.GetTraineeActivitiesDetailsListAsync(dBTMDeviceDataId, entityId, userType, centreCode, expand, filter, sort, pageIndex, pageSize);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMActivitiesDetailsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMActivitiesDetailsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMActivitiesDetailsListResponse typedBody = JsonConvert.DeserializeObject<DBTMActivitiesDetailsListResponse>(responseData);
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
        public virtual DBTMTraineeProfileResponse GetProfileDetails(long dBTMTraineeDetailId)
        {
            return Task.Run(async () => await GetProfileDetailsAsync(dBTMTraineeDetailId, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTraineeProfileResponse> GetProfileDetailsAsync(long dBTMTraineeDetailId, CancellationToken cancellationToken)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new System.ArgumentNullException("dBTMTraineeDetailId");

            string endpoint = dBTMTraineeDetailsEndpoint.GetProfileDetailsAsync(dBTMTraineeDetailId);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMTraineeProfileResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMTraineeProfileResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMTraineeProfileResponse typedBody = JsonConvert.DeserializeObject<DBTMTraineeProfileResponse>(responseData);
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

        public virtual DBTMReportsResponse GenerateAthletePdfRemark(long dBTMTraineeDetailId, string remarks)
        {
            return Task.Run(async () => await GenerateAthletePdfRemarkAsync(dBTMTraineeDetailId, remarks, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMReportsResponse> GenerateAthletePdfRemarkAsync(long dBTMTraineeDetailId, string remarks, CancellationToken cancellationToken)
        {
            if (dBTMTraineeDetailId <= 0)
                throw new System.ArgumentNullException("dBTMTraineeDetailId");

            string endpoint = dBTMTraineeDetailsEndpoint.GenerateAthletePdfRemarkAsync(dBTMTraineeDetailId, remarks);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMReportsResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new DBTMReportsResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMReportsResponse typedBody = JsonConvert.DeserializeObject<DBTMReportsResponse>(responseData);
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

        public virtual DBTMTraineeUploadResponse UploadTrainee(IFormFile file)
        {
            string endpoint = dBTMTraineeDetailsEndpoint.UploadTraineeAsync();
            HttpResponseMessage response = null;
            bool disposeResponse = true;
            try
            {
                ApiStatus status = new ApiStatus();
                var formData = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream())
                {
                    Headers = { ContentType = new MediaTypeHeaderValue(file.ContentType) }
                };
                formData.Add(fileContent, "file", file.FileName);
                response = PostResourceToEndpoint(endpoint, formData, status, CancellationToken.None);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Created:
                        return JsonConvert.DeserializeObject<DBTMTraineeUploadResponse>(response.Content.ReadAsStringAsync().Result);

                    default:
                        return JsonConvert.DeserializeObject<DBTMTraineeUploadResponse>(response.Content.ReadAsStringAsync().Result);
                }
            }
            finally
            {
                if (disposeResponse && response != null)
                    response.Dispose();
            }
        }
    }
}
