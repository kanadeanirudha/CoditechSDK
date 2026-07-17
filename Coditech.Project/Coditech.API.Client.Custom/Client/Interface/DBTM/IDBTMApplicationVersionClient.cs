using Coditech.API.Client;
using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
namespace Coditech.API.Client
{
    public interface IDBTMApplicationVersionClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTM Activity Category.
        /// </summary>
        /// <returns>DBTMApplicationVersionListResponse</returns>
        DBTMApplicationVersionListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMApplicationVersion.
        /// </summary>
        /// <param name="DBTMApplicationVersionModel">DBTMApplicationVersionModel.</param>
        /// <returns>Returns DBTMApplicationVersionResponse.</returns>
        DBTMApplicationVersionResponse CreateDBTMApplicationVersion(DBTMApplicationVersionModel body);

        /// <summary>
        /// Get DBTMApplicationVersion by dBTMApplicationVersionId.
        /// </summary>
        /// <param name="dBTMApplicationVersionId">dBTMApplicationVersionId</param>
        /// <returns>Returns DBTMApplicationVersionResponse.</returns>
        DBTMApplicationVersionResponse GetDBTMApplicationVersion(long dBTMApplicationVersionId);

        /// <summary>
        /// Update DBTMApplicationVersion.
        /// </summary>
        /// <param name="DBTMApplicationVersionModel">DBTMApplicationVersionModel.</param>
        /// <returns>Returns updated DBTMApplicationVersionResponse</returns>
        DBTMApplicationVersionResponse UpdateDBTMApplicationVersion(DBTMApplicationVersionModel body);

        /// <summary>
        /// Delete DBTMApplicationVersion.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMApplicationVersion(ParameterModel body);
    }
}