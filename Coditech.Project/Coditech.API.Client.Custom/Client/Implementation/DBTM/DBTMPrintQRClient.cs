using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.Exceptions;
using Coditech.Common.Helper.Utilities;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public class DBTMPrintQRClient : BaseClient, IDBTMPrintQRClient
    {
        DBTMPrintQREndpoint dBTMPrintQREndpoint = null;
        public DBTMPrintQRClient()
        {
            dBTMPrintQREndpoint = new DBTMPrintQREndpoint();
        }

        public virtual DBTMPrintQRListResponse DownloadPrintQR(string personIds, int generalBatchMasterId, string templateCode)
        {
            return Task.Run(async () => await DownloadPrintQRAsync(personIds, generalBatchMasterId, templateCode, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMPrintQRListResponse> DownloadPrintQRAsync(string personIds, int generalBatchMasterId, string templateCode, CancellationToken cancellationToken)
        {
            string endpoint = dBTMPrintQREndpoint.DownloadPrintQRAsync(personIds, generalBatchMasterId, templateCode);
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

        public virtual DBTMPrintQRListResponse GetDBTMPrintQRTraineeList(int generalBatchMasterId, string userType, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize)
        {
            return Task.Run(async () => await GetDBTMPrintQRTraineeListAsync(generalBatchMasterId, userType, expand, filter, sort, pageIndex, pageSize, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMPrintQRListResponse> GetDBTMPrintQRTraineeListAsync(int generalBatchMasterId, string userType, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize, CancellationToken cancellationToken)
        {
            string endpoint = dBTMPrintQREndpoint.GetDBTMPrintQRTraineeListAsync(generalBatchMasterId, userType, expand, filter, sort, pageIndex, pageSize);
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
