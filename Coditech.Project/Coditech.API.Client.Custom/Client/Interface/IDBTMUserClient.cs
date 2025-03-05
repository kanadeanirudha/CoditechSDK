using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;

namespace Coditech.API.Client
{
    public interface IDBTMUserClient : IBaseClient
    {
        /// <summary>
        /// Add IndividualRegistration.
        /// </summary>
        /// <param name="GeneralPersonModel">GeneralPersonModelModel.</param>
        /// <returns>Returns GeneralPersonResponse.</returns>
        GeneralPersonResponse IndividualRegistration(GeneralPersonModel body);
    }
}
