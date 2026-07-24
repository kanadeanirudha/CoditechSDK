using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMPrintQRClient : IBaseClient
    {    
        /// <summary>
        /// DBTMPrintQRMaster.
        /// </summary>
        /// <param name="DBTMPrintQRMasterModel">DBTMPrintQRMasterModel.</param>
        /// <returns>Returns DBTMPrintQRResponse.</returns>
        DBTMPrintQRResponse DBTMPrintQR(DBTMPrintQRModel body);

        /// <summary>
        /// Get DBTMPrintQRMaster by PersonId.
        /// </summary>
        /// <param name="PrintQRCode">PrintQRCode</param>
        /// <returns>Returns DBTMPrintQRMasterResponse.</returns>
        DBTMPrintQRListResponse GetDBTMPrintQR(string personIds);

        /// <summary>
        /// Get list of DBTMPrintQRUser.
        /// </summary>
        /// <returns>DBTMPrintQRUserListResponse</returns>
        DBTMPrintQRListResponse GetDBTMPrintQRTraineeList(int generalBatchMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);
    }
}
