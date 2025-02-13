using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMTestAgent
    {
        /// <summary>
        /// Get list of DBTM Test.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMTestListViewModel</returns>
        DBTMTestListViewModel GetDBTMTestList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTMTest.
        /// </summary>
        /// <param name="dBTMTestViewModel">DBTM Test View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMTestViewModel CreateDBTMTest(DBTMTestViewModel dBTMTestViewModel);

        /// <summary>
        /// Get DBTMTest by dBTMTestMasterId.
        /// </summary>
        /// <param name="dBTMTestMasterId">dBTMTestMasterId</param>
        /// <returns>Returns DBTMDeviceViewModel.</returns>
        DBTMTestViewModel GetDBTMTest(int dBTMTestMasterId);

        /// <summary>
        /// Update DBTMTest.
        /// </summary>
        /// <param name="dBTMTestViewModel">dBTMTestViewModel.</param>
        /// <returns>Returns updated DBTMTestViewModel</returns>
        DBTMTestViewModel UpdateDBTMTest(DBTMTestViewModel dBTMTestViewModel);

        /// <summary>
        /// Delete DBTMTest.
        /// </summary>
        /// <param name="dBTMTestMasterIds">dBTMTestMasterIdIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMTest(string dBTMTestMasterIds, out string errorMessage);
        DBTMTestParameterListViewModel DBTMTestParameter();
    }
}
