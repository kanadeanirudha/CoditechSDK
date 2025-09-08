using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMGraphAgent
    {
        /// <summary>
        /// Get list of DBTM Graph.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMGraphMasterListViewModel</returns>
        DBTMGraphMasterListViewModel GetDBTMGraphList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTM Graph Master.
        /// </summary>
        /// <param name="dBTMGraphMasterViewModel">DBTM Graph Master View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMGraphMasterViewModel CreateDBTMGraph(DBTMGraphMasterViewModel dBTMGraphMasterViewModel);

        /// <summary>
        /// Get DBTMGraphMaster by dBTMGraphMasterId.
        /// </summary>
        /// <param name="graphCode">graphCode</param>
        /// <returns>Returns DBTMGraphMasterViewModel.</returns>
        DBTMGraphMasterViewModel GetDBTMGraph(string graphCode);

        /// <summary>
        /// Update DBTM Graph Master.
        /// </summary>
        /// <param name="dBTMGraphMasterViewModel">dBTMGraphMasterViewModel.</param>
        /// <returns>Returns updated DBTMGraphMasterViewModel</returns>
        DBTMGraphMasterViewModel UpdateDBTMGraph(DBTMGraphMasterViewModel dBTMGraphMasterViewModel);

        /// <summary>
        /// Delete DBTM Graph Master.
        /// </summary>
        /// <param name="dBTMGraphMasterId">dBTMGraphMasterId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMGraph(string graphCode, out string errorMessage);
        DBTMTestListViewModel DBTMGraphTestCode();
    }
}
