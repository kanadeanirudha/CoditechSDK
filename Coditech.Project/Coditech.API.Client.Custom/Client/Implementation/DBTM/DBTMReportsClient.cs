using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;

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

        public virtual DBTMBatchWiseReportsListResponse BatchWiseReports(int generalBatchMasterId, DateTime FromDate, DateTime ToDate)
        {
            return Task.Run(async () => await BatchWiseReportsAsync(generalBatchMasterId,FromDate,ToDate, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMBatchWiseReportsListResponse> BatchWiseReportsAsync(int generalBatchMasterId, DateTime FromDate, DateTime ToDate, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.BatchWiseReportsAsync(generalBatchMasterId, FromDate,ToDate);
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
                    var objectResponse = await ReadObjectResponseAsync<DBTMBatchWiseReportsListResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else if (status_ == 204)
                {
                    return new DBTMBatchWiseReportsListResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    DBTMBatchWiseReportsListResponse typedBody = JsonConvert.DeserializeObject<DBTMBatchWiseReportsListResponse>(responseData);
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

        public virtual DBTMTestWiseReportsListResponse TestWiseReports(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, long entityId)
        {
            return Task.Run(async () => await TestWiseReportsAsync(dBTMTestMasterId, dBTMTraineeDetailId,FromDate,ToDate, entityId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMTestWiseReportsListResponse> TestWiseReportsAsync(int dBTMTestMasterId, long dBTMTraineeDetailId, DateTime FromDate, DateTime ToDate, long entityId, CancellationToken cancellationToken)
        {
            string endpoint = dBTMReportsEndpoint.TestWiseReportsAsync(dBTMTestMasterId,dBTMTraineeDetailId, FromDate,ToDate,entityId);
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
    }
}
