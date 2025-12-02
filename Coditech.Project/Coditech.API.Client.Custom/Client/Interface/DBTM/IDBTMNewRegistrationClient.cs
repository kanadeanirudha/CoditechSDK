using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;

namespace Coditech.API.Client
{
    public interface IDBTMNewRegistrationClient : IBaseClient
    {
        /// <summary>
        /// Add DBTMCentreRegistration.
        /// </summary>
        /// <param name="DBTMNewRegistrationModel">DBTMNewRegistrationModel.</param>
        /// <returns>Returns DBTMDeviceResponse.</returns>
        DBTMNewRegistrationResponse DBTMCentreRegistration(DBTMNewRegistrationModel body);

        /// <summary>
        /// Add TrainerRegistration.
        /// </summary>
        /// <param name="DBTMNewRegistrationModel">DBTMNewRegistrationModel.</param>
        /// <returns>Returns DBTMDeviceResponse.</returns>
        DBTMNewRegistrationResponse TrainerRegistration(DBTMNewRegistrationModel body);
        /// <summary>
        /// Get Country by joiningCode.
        /// </summary>
        /// <param name="joiningCode">joiningCode</param>
        /// <returns>Returns Response.</returns>
        DBTMNewRegistrationListResponse GetGeneralTrainerByJoiningCode(string joiningCode);


    }
}
