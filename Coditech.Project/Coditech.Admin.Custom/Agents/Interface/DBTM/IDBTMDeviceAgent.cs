using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMDeviceAgent
    {
        /// <summary>
        /// Get list of DBTM Device.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMDeviceListViewModel</returns>
        DBTMDeviceListViewModel GetDBTMDeviceList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTMDevice.
        /// </summary>
        /// <param name="dBTMDeviceViewModel">DBTM Device View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMDeviceViewModel CreateDBTMDevice(DBTMDeviceViewModel dBTMDeviceViewModel);

        /// <summary>
        /// Get DBTMDevice by dBTMDeviceId.
        /// </summary>
        /// <param name="dBTMDeviceId">dBTMDeviceId</param>
        /// <returns>Returns DBTMDeviceViewModel.</returns>
        DBTMDeviceViewModel GetDBTMDevice(long dBTMDeviceId);

        /// <summary>
        /// Update DBTMDevice.
        /// </summary>
        /// <param name="dBTMDeviceViewModel">dBTMDeviceViewModel.</param>
        /// <returns>Returns updated DBTMDeviceViewModel</returns>
        DBTMDeviceViewModel UpdateDBTMDevice(DBTMDeviceViewModel dBTMDeviceViewModel);

        /// <summary>
        /// Delete DBTMDevice.
        /// </summary>
        /// <param name="dBTMDeviceIds">dBTMDeviceIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMDevice(string dBTMDeviceIds, out string errorMessage);
    }
}
