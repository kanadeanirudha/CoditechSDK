using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMBatchActivityAgent
    {
        ///// <summary>
        ///// Get list of DBTM Test.
        ///// </summary>
        ///// <param name="dataTableModel">DataTable ViewModel.</param>
        ///// <returns>DBTMTestListViewModel</returns>
        GeneralBatchListViewModel GetBatchList(DataTableViewModel dataTableModel);


        /// <summary>
        /// Get list of DBTMBatchActivity.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMBatchActivityListViewModel</returns>
        DBTMBatchActivityListViewModel GetDBTMBatchActivityList(int generalBatchMasterId, /*bool IsAssociated, */DataTableViewModel dataTableModel);

        DBTMBatchActivityViewModel DBTMBatchActivity(int generalBatchMasterId);

        /// <summary>
        /// Create DBTMBatchActivity.
        /// </summary>
        /// <param name="dBTMBatchActivityViewModel">DBTM Batch Activity View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMBatchActivityViewModel CreateDBTMBatchActivity(DBTMBatchActivityViewModel dBTMBatchActivityViewModel);

        /// <summary>
        /// Delete DBTMBatchActivity.
        /// </summary>
        /// <param name="dBTMBatchActivityIds">dBTMBatchActivityIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMBatchActivity(string dBTMBatchActivityIds, out string errorMessage);
    }
}
