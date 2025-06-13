using Coditech.API.Endpoint;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.API.Model;
using Coditech.Common.Exceptions;
using Newtonsoft.Json;
using System.Net;
using Coditech.Common.Logger;
using System.Diagnostics;

namespace Coditech.API.Client
{
    public class DBTMUserClient : BaseClient, IDBTMUserClient
    {
        DBTMUserEndpoint dBTMUserEndpoint = null;
        public DBTMUserClient()
        {
            dBTMUserEndpoint = new DBTMUserEndpoint();
        }

        public virtual GeneralPersonResponse IndividualRegistration(GeneralPersonModel body)
        {
            return Task.Run(async () => await IndividualRegistrationAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GeneralPersonResponse> IndividualRegistrationAsync(GeneralPersonModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMUserEndpoint.IndividualRegistrationAsync();
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
                            ObjectResponseResult<GeneralPersonResponse> objectResponseResult2 = await ReadObjectResponseAsync<GeneralPersonResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<GeneralPersonResponse> objectResponseResult = await ReadObjectResponseAsync<GeneralPersonResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            GeneralPersonResponse result = JsonConvert.DeserializeObject<GeneralPersonResponse>(value);
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

        public virtual GeneralPersonResponse TraineeRegistration(GeneralPersonModel body)
        {
            return Task.Run(async () => await TraineeRegistrationAsync(body, CancellationToken.None)).GetAwaiter().GetResult();
        }

        public virtual async Task<GeneralPersonResponse> TraineeRegistrationAsync(GeneralPersonModel body, CancellationToken cancellationToken)
        {
            string endpoint = dBTMUserEndpoint.TraineeRegistrationAsync();
            HttpResponseMessage response = null;
            bool disposeResponse = true;
            try
            {
                new CoditechLogging().LogMessage("TraineeRegistrationAsync 1:"+ endpoint, "TraineeRegistration", TraceLevel.Error);
                ApiStatus status = new ApiStatus();
                response = await PostResourceToEndpointAsync(endpoint, JsonConvert.SerializeObject(body), status, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                Dictionary<string, IEnumerable<string>> dictionary = BindHeaders(response);
                new CoditechLogging().LogMessage("TraineeRegistrationAsync 2:" + JsonConvert.SerializeObject(response), "TraineeRegistration", TraceLevel.Error);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        {
                            ObjectResponseResult<GeneralPersonResponse> objectResponseResult2 = await ReadObjectResponseAsync<GeneralPersonResponse>(response, BindHeaders(response), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult2.Object == null)
                            {
                                throw new CoditechException(objectResponseResult2.Object.ErrorCode, objectResponseResult2.Object.ErrorMessage);
                            }

                            return objectResponseResult2.Object;
                        }
                    case HttpStatusCode.Created:
                        {
                            ObjectResponseResult<GeneralPersonResponse> objectResponseResult = await ReadObjectResponseAsync<GeneralPersonResponse>(response, dictionary, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                            if (objectResponseResult.Object == null)
                            {
                                throw new CoditechException(objectResponseResult.Object.ErrorCode, objectResponseResult.Object.ErrorMessage);
                            }

                            return objectResponseResult.Object;
                        }
                    default:
                        {
                            string value = ((response.Content != null) ? (await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false)) : null);
                            GeneralPersonResponse result = JsonConvert.DeserializeObject<GeneralPersonResponse>(value);
                            new CoditechLogging().LogMessage("TraineeRegistrationAsync 3:" + JsonConvert.SerializeObject(result), "TraineeRegistration", TraceLevel.Error);

                            UpdateApiStatus(result, status, response);
                            throw new CoditechException(status.ErrorCode, status.ErrorMessage, status.StatusCode);
                        }
                }
            }
            catch (Exception ex)
            {
                new CoditechLogging().LogMessage(ex, "TraineeRegistration", TraceLevel.Error);
                throw new CoditechException(null, null, null);
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
