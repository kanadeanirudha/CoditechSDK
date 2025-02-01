using Coditech.API.Endpoint;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Exceptions;

using Newtonsoft.Json;

using System.Net;

namespace Coditech.API.Client
{
    public class DBTMNewRegistrationClient : BaseClient, IDBTMNewRegistrationClient
    {
        DBTMNewRegistrationEndpoint dBTMNewRegistrationEndpoint = null;
        public DBTMNewRegistrationClient()
        {
            dBTMNewRegistrationEndpoint = new DBTMNewRegistrationEndpoint();
        }
        
        public virtual DBTMNewRegistrationResponse DBTMNewRegistration(DBTMNewRegistrationModel body)
        {
            return Task.Run(async () => await NewRegistrationAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<DBTMNewRegistrationResponse> NewRegistrationAsync(DBTMNewRegistrationModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMNewRegistrationEndpoint.NewRegistrationAsync();
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
                            ObjectResponseResult<DBTMNewRegistrationResponse> objectResponseResult2 = await ReadObjectResponseAsync<DBTMNewRegistrationResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<DBTMNewRegistrationResponse> objectResponseResult = await ReadObjectResponseAsync<DBTMNewRegistrationResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            DBTMNewRegistrationResponse result = JsonConvert.DeserializeObject<DBTMNewRegistrationResponse>(value);
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
    }
}
