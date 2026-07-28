using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMPrintQRAgent
    {
        /// <summary>
        /// Get DBTMPrintQRMaster by dBTMPrintQRMasterId.
        /// </summary>
        /// <param name="PrintQRCode">PrintQRCode</param>
        /// <returns>Returns DBTMPrintQRMasterViewModel.</returns>
        DBTMPrintQRListViewModel DownloadPrintQR(string personIds);

        /// <summary>
        /// Get list of Associated PrintQR.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMPrintQRUserListViewModel</returns>
        DBTMPrintQRListViewModel GetDBTMPrintQRTraineeList(int dBTMPrintQRMasterId, DataTableViewModel dataTableModel);

    }
}
