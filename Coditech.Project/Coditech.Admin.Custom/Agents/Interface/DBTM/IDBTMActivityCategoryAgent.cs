using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMActivityCategoryAgent
    {
        /// <summary>
        /// Get list of DBTM Activity Category.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMActivityCategoryListViewModel</returns>
        DBTMActivityCategoryListViewModel GetDBTMActivityCategoryList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTMActivityCategory.
        /// </summary>
        /// <param name="dBTMActivityCategoryViewModel"> DBTM Activity Category View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMActivityCategoryViewModel CreateDBTMActivityCategory(DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel);

        /// <summary>
        /// Get DBTMActivityCategory by DBTMActivityCategoryId.
        /// </summary>
        /// <param name="dBTMActivityCategoryId">dBTMActivityCategoryId</param>
        /// <returns>Returns DBTMActivityCategoryViewModel.</returns>
        DBTMActivityCategoryViewModel GetDBTMActivityCategory(short dBTMActivityCategoryId);

        /// <summary>
        /// Update DBTMActivityCategory.
        /// </summary>
        /// <param name="dBTMActivityCategoryViewModel">dBTMActivityCategoryViewModel.</param>
        /// <returns>Returns updated DBTMActivityCategoryViewModel</returns>
        DBTMActivityCategoryViewModel UpdateDBTMActivityCategory(DBTMActivityCategoryViewModel dBTMActivityCategoryViewModel);

        /// <summary>
        /// Delete DBTMActivityCategory.
        /// </summary>
        /// <param name="dBTMActivityCategoryId">dBTMActivityCategoryId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMActivityCategory(string dBTMActivityCategoryId, out string errorMessage);
        DBTMActivityCategoryListResponse GetDBTMActivityCategoryList();
    }
}
