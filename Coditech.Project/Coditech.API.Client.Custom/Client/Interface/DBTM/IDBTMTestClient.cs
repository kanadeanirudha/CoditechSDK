using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMTestClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMTest.
        /// </summary>
        /// <returns>DBTMTestListResponse</returns>
        DBTMTestListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMTest.
        /// </summary>
        /// <param name="DBTMTestModel">DBTMTestModel.</param>
        /// <returns>Returns DBTMTestResponse.</returns>
        DBTMTestResponse CreateDBTMTest(DBTMTestModel body);

        /// <summary>
        /// Get DBTMTest by dBTMTestMasterId.
        /// </summary>
        /// <param name="dBTMTestMasterId">dBTMTestMasterId</param>
        /// <returns>Returns DBTMTestResponse.</returns>
        DBTMTestResponse GetDBTMTest(int dBTMTestMasterId);

        /// <summary>
        /// Update DBTMTest.
        /// </summary>
        /// <param name="DBTMTestModel">DBTMTestModel.</param>
        /// <returns>Returns updated DBTMTestResponse</returns>
        DBTMTestResponse UpdateDBTMTest(DBTMTestModel model);

        /// <summary>
        /// Delete DBTMTest.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMTest(ParameterModel body);
        DBTMTestParameterListResponse GetDBTMTestParameter();
    }
}
