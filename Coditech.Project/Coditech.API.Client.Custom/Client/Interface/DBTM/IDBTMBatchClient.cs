using Coditech.Common.API.Model.Response;

namespace Coditech.API.Client
{
    public interface IDBTMBatchClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMBatchList.
        /// </summary>
        /// <returns>DBTMBatchListResponse</returns>
        DBTMBatchListResponse GetBatchList(long entityId,string userType);
    }
}
