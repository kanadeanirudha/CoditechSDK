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
        DBTMNewRegistrationViewModel DBTMNewRegistration(DBTMNewRegistrationViewModel dBTMNewRegistrationViewModel);
    }
}
