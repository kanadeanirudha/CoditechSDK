using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMPrintQRAgent
    {

        /// <summary>
        /// DBTM PrintQR Master.
        /// </summary>
        /// <param name="dBTMPrintQRMasterViewModel">DBTM PrintQR Master View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMPrintQRViewModel DBTMPrintQR(DBTMPrintQRViewModel dBTMPrintQRMasterViewModel);

        /// <summary>
        /// Get DBTMPrintQRMaster by dBTMPrintQRMasterId.
        /// </summary>
        /// <param name="PrintQRCode">PrintQRCode</param>
        /// <returns>Returns DBTMPrintQRMasterViewModel.</returns>
        DBTMPrintQRListViewModel GetDBTMPrintQR(string personIds);

        /// <summary>
        /// Get list of Associated PrintQR.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMPrintQRUserListViewModel</returns>
        DBTMPrintQRListViewModel GetDBTMPrintQRTraineeList(int dBTMPrintQRMasterId, DataTableViewModel dataTableModel);

    }
}
