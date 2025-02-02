using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMActivityCategoryClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTM Activity Category.
        /// </summary>
        /// <returns>DBTMActivityCategoryListResponse</returns>
        DBTMActivityCategoryListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMActivityCategory.
        /// </summary>
        /// <param name="DBTMActivityCategoryModel">DBTMActivityCategoryModel.</param>
        /// <returns>Returns DBTMActivityCategoryResponse.</returns>
        DBTMActivityCategoryResponse CreateDBTMActivityCategory(DBTMActivityCategoryModel body);

        /// <summary>
        /// Get DBTMActivityCategory by dBTMActivityCategoryId.
        /// </summary>
        /// <param name="dBTMActivityCategoryId">dBTMActivityCategoryId</param>
        /// <returns>Returns DBTMActivityCategoryResponse.</returns>
        DBTMActivityCategoryResponse GetDBTMActivityCategory(short dBTMActivityCategoryId);

        /// <summary>
        /// Update DBTMActivityCategory.
        /// </summary>
        /// <param name="DBTMActivityCategoryModel">DBTMActivityCategoryModel.</param>
        /// <returns>Returns updated DBTMActivityCategoryResponse</returns>
        DBTMActivityCategoryResponse UpdateDBTMActivityCategory(DBTMActivityCategoryModel body);

        /// <summary>
        /// Delete DBTMActivityCategory.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMActivityCategory(ParameterModel body);
    }
}
