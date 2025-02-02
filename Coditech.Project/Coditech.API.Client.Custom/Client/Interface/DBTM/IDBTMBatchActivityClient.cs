using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMBatchActivityClient : IBaseClient
    {

        /// <summary>
        /// Get list of DBTMBatchActivity.
        /// </summary>
        /// <returns>DBTMBatchActivityListResponse</returns>
        DBTMBatchActivityListResponse GetDBTMBatchActivityList(int generalBatchMasterId, bool IsAssociated, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMBatchActivity.
        /// </summary>
        /// <param name="DBTMBatchActivityModel">DBTMBatchActivityModel.</param>
        /// <returns>Returns DBTMBatchActivityResponse.</returns>
        DBTMBatchActivityResponse CreateDBTMBatchActivity(DBTMBatchActivityModel body);

        /// <summary>
        /// Delete DBTMBatchActivity.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMBatchActivity(ParameterModel body);
    }
}
