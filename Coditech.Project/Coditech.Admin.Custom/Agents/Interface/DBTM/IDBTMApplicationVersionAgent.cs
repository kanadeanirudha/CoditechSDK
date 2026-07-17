using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMApplicationVersionAgent
    {
        /// <summary>
        /// Get list of DBTM Application Version.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMApplicationVersionListViewModel</returns>
        DBTMApplicationVersionListViewModel GetDBTMApplicationVersionList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTMApplicationVersion.
        /// </summary>
        /// <param name="DBTMApplicationVersionViewModel"> DBTMApplicationVersionViewModel.</param>
        /// <returns>Returns created model.</returns>
        DBTMApplicationVersionViewModel CreateDBTMApplicationVersion(DBTMApplicationVersionViewModel DBTMApplicationVersionViewModel);

        /// <summary>
        /// Get DBTMApplicationVersion by DBTMApplicationVersionId.
        /// </summary>
        /// <param name="DBTMApplicationVersionId">DBTMApplicationVersionId</param>
        /// <returns>Returns DBTMApplicationVersionViewModel.</returns>
        DBTMApplicationVersionViewModel GetDBTMApplicationVersion(long DBTMApplicationVersionId);

        /// <summary>
        /// Update DBTMApplicationVersion.
        /// </summary>
        /// <param name="DBTMApplicationVersionViewModel">DBTMApplicationVersionViewModel.</param>
        /// <returns>Returns updated DBTMApplicationVersionViewModel</returns>
        DBTMApplicationVersionViewModel UpdateDBTMApplicationVersion(DBTMApplicationVersionViewModel DBTMApplicationVersionViewModel);

        /// <summary>
        /// Delete DBTMApplicationVersion.
        /// </summary>
        /// <param name="DBTMApplicationVersionId">DBTMApplicationVersionId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMApplicationVersion(string DBTMApplicationVersionId, out string errorMessage);
    }
}
