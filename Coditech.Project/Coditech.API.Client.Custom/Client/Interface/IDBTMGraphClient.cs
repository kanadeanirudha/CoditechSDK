using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMGraphClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTM Graph.
        /// </summary>
        /// <returns>DBTMGraphMasterListResponse</returns>
        DBTMGraphMasterListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMGraphMaster.
        /// </summary>
        /// <param name="DBTMGraphMasterModel">DBTMGraphMasterModel.</param>
        /// <returns>Returns DBTMGraphMasterResponse.</returns>
        DBTMGraphMasterResponse CreateDBTMGraph(DBTMGraphMasterModel body);

        /// <summary>
        /// Get DBTMGraphMaster by DBTMGraphMasterId.
        /// </summary>
        /// <param name="graphCode">graphCode</param>
        /// <returns>Returns DBTMGraphMasterResponse.</returns>
        DBTMGraphMasterResponse GetDBTMGraph(string graphCode);

        /// <summary>
        /// Update DBTMGraphMaster.
        /// </summary>
        /// <param name="DBTMGraphMasterModel">DBTMGraphMasterModel.</param>
        /// <returns>Returns updated DBTMGraphMasterResponse</returns>
        DBTMGraphMasterResponse UpdateDBTMGraph(DBTMGraphMasterModel body);

        /// <summary>
        /// Delete DBTMGraphMaster.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMGraph(ParameterModel body);
        DBTMTestListResponse GetDBTMGraphTestCode();
    }
}
