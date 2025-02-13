using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMNewRegistrationAgent
    {
        /// <summary>
        /// Add DBTM New Registration.
        /// </summary>
        /// <param name="dBTMNewRegistrationViewModel">DBTM New Registration View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMNewRegistrationViewModel DBTMCentreRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel);

        /// <summary>
        /// Add Trainer Registration.
        /// </summary>
        /// <param name="dBTMNewRegistrationViewModel">DBTM New Registration View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMNewRegistrationViewModel TrainerRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel);

        /// <summary>
        /// Add Individual Registration.
        /// </summary>
        /// <param name="dBTMNewRegistrationViewModel">DBTM New Registration View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMNewRegistrationViewModel IndividualRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel);
    }
}
