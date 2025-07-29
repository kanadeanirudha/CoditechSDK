using Coditech.Common.API.Model.Response;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMBatchClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMBatchList.
        /// </summary>
        /// <returns>DBTMBatchListResponse</returns>
        DBTMBatchListResponse GetBatchList(long entityId,string userType);
        /// <summary>
        /// Get list of GeneralBatchUser.
        /// </summary>
        /// <returns>GeneralBatchUserListResponse</returns>
        GeneralBatchUserListResponse GetDBTMBatchUserList(string selectedCentreCode, long generalTrainerMasterId, int generalBatchMasterId);
    }
}
