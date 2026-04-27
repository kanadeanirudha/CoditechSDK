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
        DBTMGraphMasterListResponse GetDBTMGraph(int dBTMTestMasterId);

        /// <summary>
        /// Get ActivityListViewSequence by DBTMTestParameterListViewSequenceId.
        /// </summary>
        /// <param name="DBTMTestParameterListViewSequenceId">DBTMTestParameterListViewSequenceId</param>
        /// <returns>Returns DBTMTestResponse.</returns>
        DBTMActivityListViewSequenceResponse GetActivityListViewSequence(int dBTMTestParameterListViewSequenceId);

        /// <summary>
        /// Update ActivityListViewSequence.
        /// </summary>
        /// <param name="DBTMTestModel">DBTMTestModel.</param>
        /// <returns>Returns updated DBTMTestResponse</returns>
        DBTMActivityListViewSequenceResponse UpdateActivityListViewSequence(DBTMActivityListViewSequenceModel model);

        /// <summary>
        /// Get ActivityListViewSequence by dBTMTestMasterId.
        /// </summary>
        /// <param name="dBTMTestMasterId">dBTMTestMasterId</param>
        /// <returns>Returns DBTMTestResponse.</returns>   
        DBTMActivityListViewSequenceListResponse GetActivityListViewSequenceList(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Delete DBTMActivityListViewSequence.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteActivityListViewSequence(ParameterModel body);

        /// <summary>
        /// Update Sequence Number.
        /// </summary>
        /// <param name="DBTMActivityListViewSequenceModel">DBTMActivityListViewSequenceModel.</param>
        /// <returns>Returns DBTMActivityListViewSequenceResponse.</returns>
        DBTMActivityListViewSequenceResponse UpdateSequenceNumber(DBTMActivityListViewSequenceModel body);

        /// <summary>
        /// Create DBTMActivityListViewSequence.
        /// </summary>
        /// <param name="DBTMActivityListViewSequenceModel">DBTMActivityListViewSequenceModel.</param>
        /// <returns>Returns DBTMActivityListViewSequenceResponse.</returns>
        DBTMActivityListViewSequenceResponse CreateActivityListViewSequence(DBTMActivityListViewSequenceModel body);

        /// <summary>
        /// Get ActivityVerticalViewSequence by DBTMTestParameterListViewSequenceId.
        /// </summary>
        /// <param name="DBTMTestParameterVerticalViewSequenceId">DBTMTestParameterVerticalViewSequenceId</param>
        /// <returns>Returns DBTMActivityVerticalViewSequenceResponse.</returns>
        DBTMActivityVerticalViewSequenceResponse GetActivityVerticalViewSequence(int dBTMTestParameterVerticalViewSequenceId);

        /// <summary>
        /// Update ActivityVerticalViewSequence.
        /// </summary>
        /// <param name="DBTMActivityVerticalViewSequenceModel">DBTMActivityVerticalViewSequenceModel.</param>
        /// <returns>Returns updated DBTMActivityVerticalViewSequenceResponse</returns>
        DBTMActivityVerticalViewSequenceResponse UpdateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel model);

        /// <summary>
        /// Get ActivityVerticalViewSequence by dBTMTestMasterId.
        /// </summary>
        /// <param name="dBTMTestMasterId">dBTMTestMasterId</param>
        /// <returns>Returns DBTMActivityVerticalViewSequenceListResponse.</returns>   
        DBTMActivityVerticalViewSequenceListResponse GetActivityVerticalViewSequenceList(int dBTMTestMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Delete DBTMActivityVerticalViewSequence.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteActivityVerticalViewSequence(ParameterModel body);

        /// <summary>
        /// Update Vertical Sequence Number.
        /// </summary>
        /// <param name="DBTMActivityVerticalViewSequenceModel">DBTMActivityVerticalViewSequenceModel.</param>
        /// <returns>Returns DBTMActivityVerticalViewSequenceResponse.</returns>
        DBTMActivityVerticalViewSequenceResponse UpdateVerticalSequenceNumber(DBTMActivityVerticalViewSequenceModel body);

        /// <summary>
        /// Create DBTMActivityVerticalViewSequence.
        /// </summary>
        /// <param name="DBTMActivityVerticalViewSequenceModel">DBTMActivityVerticalViewSequenceModel.</param>
        /// <returns>Returns DBTMActivityVerticalViewSequenceResponse.</returns>
        DBTMActivityVerticalViewSequenceResponse CreateActivityVerticalViewSequence(DBTMActivityVerticalViewSequenceModel body);
        DBTMCentreWiseTestListResponse GetTestsByCentreCode(string centreCode);
    }
}
