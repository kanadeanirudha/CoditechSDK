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

        /// <summary>
        /// Get GraphVerticalViewSequence by DBTMGraphParameterListViewSequenceId.
        /// </summary>
        /// <param name="DBTMGraphParameterVerticalViewSequenceId">DBTMGraphParameterVerticalViewSequenceId</param>
        /// <returns>Returns DBTMGraphVerticalViewSequenceResponse.</returns>
        DBTMGraphVerticalViewSequenceResponse GetGraphVerticalViewSequence(int dBTMGraphParameterVerticalViewSequenceId);

        /// <summary>
        /// Update GraphVerticalViewSequence.
        /// </summary>
        /// <param name="DBTMGraphVerticalViewSequenceModel">DBTMGraphVerticalViewSequenceModel.</param>
        /// <returns>Returns updated DBTMGraphVerticalViewSequenceResponse</returns>
        DBTMGraphVerticalViewSequenceResponse UpdateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceModel model);

        /// <summary>
        /// Get GraphVerticalViewSequence by dBTMGraphMasterId.
        /// </summary>
        /// <param name="dBTMGraphMasterId">dBTMGraphMasterId</param>
        /// <returns>Returns DBTMGraphVerticalViewSequenceListResponse.</returns>   
        DBTMGraphVerticalViewSequenceListResponse GetGraphVerticalViewSequenceList(int dBTMGraphMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Delete DBTMGraphVerticalViewSequence.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteGraphVerticalViewSequence(ParameterModel body);

        /// <summary>
        /// Update Vertical Sequence Number.
        /// </summary>
        /// <param name="DBTMGraphVerticalViewSequenceModel">DBTMGraphVerticalViewSequenceModel.</param>
        /// <returns>Returns DBTMGraphVerticalViewSequenceResponse.</returns>
        DBTMGraphVerticalViewSequenceResponse UpdateGraphVerticalSequenceNumber(DBTMGraphVerticalViewSequenceModel body);

        /// <summary>
        /// Create DBTMGraphVerticalViewSequence.
        /// </summary>
        /// <param name="DBTMGraphVerticalViewSequenceModel">DBTMGraphVerticalViewSequenceModel.</param>
        /// <returns>Returns DBTMGraphVerticalViewSequenceResponse.</returns>
        DBTMGraphVerticalViewSequenceResponse CreateGraphVerticalViewSequence(DBTMGraphVerticalViewSequenceModel body);
    }
}
