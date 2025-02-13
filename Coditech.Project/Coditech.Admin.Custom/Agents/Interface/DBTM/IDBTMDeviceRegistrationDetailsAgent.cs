using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMDeviceRegistrationDetailsAgent
    {
        /// <summary>
        /// Get list of DBTMDeviceRegistrationDetails.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMDeviceRegistrationDetailsListViewModel</returns>
        DBTMDeviceRegistrationDetailsListViewModel GetDBTMDeviceRegistrationDetailsList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create RegistrationDetails.
        /// </summary>
        /// <param name="dBTMDeviceRegistrationDetailsViewModel">DBTM Device Registration Details View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMDeviceRegistrationDetailsViewModel CreateRegistrationDetails(DBTMDeviceRegistrationDetailsViewModel dBTMDeviceRegistrationDetailsViewModel);

        /// <summary>
        /// Get RegistrationDetails by dBTMDeviceRegistrationDetailId.
        /// </summary>
        /// <param name="dBTMDeviceRegistrationDetailId">dBTMDeviceRegistrationDetailId</param>
        /// <returns>Returns DBTMDeviceViewModel.</returns>
        DBTMDeviceRegistrationDetailsViewModel GetRegistrationDetails(long dBTMDeviceRegistrationDetailId);

        /// <summary>
        /// Update RegistrationDetails.
        /// </summary>
        /// <param name="dBTMDeviceRegistrationDetailsViewModel">dBTMDeviceRegistrationDetailsViewModel.</param>
        /// <returns>Returns updated DBTMDeviceRegistrationDetailsViewModel</returns>
        DBTMDeviceRegistrationDetailsViewModel UpdateRegistrationDetails(DBTMDeviceRegistrationDetailsViewModel dBTMDeviceRegistrationDetailsViewModel);

        /// <summary>
        /// Delete RegistrationDetails.
        /// </summary>
        /// <param name="dBTMDeviceRegistrationDetailIds">dBTMDeviceRegistrationDetailIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteRegistrationDetails(string dBTMDeviceRegistrationDetailIds, out string errorMessage);
    }
}
