using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;

namespace Coditech.API.Client
{
    public interface IDBTMNewRegistrationClient : IBaseClient
    {
        /// <summary>
        /// Add DBTMNewRegistration.
        /// </summary>
        /// <param name="DBTMNewRegistrationModel">DBTMNewRegistrationModel.</param>
        /// <returns>Returns DBTMDeviceResponse.</returns>
        DBTMNewRegistrationResponse DBTMNewRegistration(DBTMNewRegistrationModel body);
    }
}
