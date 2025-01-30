using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;

using Newtonsoft.Json;

namespace Coditech.API.Client
{
    public class BioradMedisyMediaManagerClient : BaseClient, IBioradMedisyMediaManagerClient
    {
        BioradMedisyMediaManagerEndpoint mediaSettingMasterEndpoint = null;
        public BioradMedisyMediaManagerClient()
        {
            mediaSettingMasterEndpoint = new BioradMedisyMediaManagerEndpoint();
        }
        public BioradMedisyMediaResponse GetMediaDetails(long mediaId, long entityId)
        {
            return Task.Run(async () => await GetMediaDetailsAsync(mediaId, entityId, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public async Task<BioradMedisyMediaResponse> GetMediaDetailsAsync(long mediaId, long entityId, CancellationToken cancellationToken)
        {
            if (mediaId <= 0)
                throw new System.ArgumentNullException("mediaId");

            string endpoint = mediaSettingMasterEndpoint.GetMediaDetailsAsync(mediaId, entityId);
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
                    var objectResponse = await ReadObjectResponseAsync<BioradMedisyMediaResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 204)
                {
                    return new BioradMedisyMediaResponse();
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    BioradMedisyMediaResponse typedBody = JsonConvert.DeserializeObject<BioradMedisyMediaResponse>(responseData);
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

        public BioradMedisyMediaResponse UpdateFileApprovalFlow(BioradMedisyMediaModel body)
        {
            return Task.Run(async () => await UpdateFileApprovalFlowAsync(body, System.Threading.CancellationToken.None)).GetAwaiter().GetResult();
        }

        public async Task<BioradMedisyMediaResponse> UpdateFileApprovalFlowAsync(BioradMedisyMediaModel body, System.Threading.CancellationToken cancellationToken)
        {
            string endpoint = mediaSettingMasterEndpoint.UpdateFileApprovalFlowAsync();
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
                    var objectResponse = await ReadObjectResponseAsync<BioradMedisyMediaResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                if (status_ == 201)
                {
                    var objectResponse = await ReadObjectResponseAsync<BioradMedisyMediaResponse>(response, headers_, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new CoditechException(objectResponse.Object.ErrorCode, objectResponse.Object.ErrorMessage);
                    }
                    return objectResponse.Object;
                }
                else
                {
                    string responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    BioradMedisyMediaResponse typedBody = JsonConvert.DeserializeObject<BioradMedisyMediaResponse>(responseData);
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
