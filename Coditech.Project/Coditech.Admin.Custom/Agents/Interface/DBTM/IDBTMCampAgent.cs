using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMCampAgent
    {
        /// <summary>
        /// Get list of DBTM Camp.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMCampListViewModel</returns>
        DBTMCampListViewModel GetDBTMCampList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTM Camp Master.
        /// </summary>
        /// <param name="dBTMCampMasterViewModel">DBTM Camp Master View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMCampMasterViewModel CreateDBTMCamp(DBTMCampMasterViewModel dBTMCampMasterViewModel);

        /// <summary>
        /// Get DBTMCampMaster by dBTMCampMasterId.
        /// </summary>
        /// <param name="CampCode">CampCode</param>
        /// <returns>Returns DBTMCampMasterViewModel.</returns>
        DBTMCampMasterViewModel GetDBTMCamp(int dBTMCampMasterId);

        /// <summary>
        /// Update DBTM Camp Master.
        /// </summary>
        /// <param name="dBTMCampMasterViewModel">dBTMCampMasterViewModel.</param>
        /// <returns>Returns updated DBTMCampMasterViewModel</returns>
        DBTMCampMasterViewModel UpdateDBTMCamp(DBTMCampMasterViewModel dBTMCampMasterViewModel);

        /// <summary>
        /// Delete DBTM Camp Master.
        /// </summary>
        /// <param name="dBTMCampMasterId">dBTMCampMasterId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMCamp(string CampCode, out string errorMessage);

        /// <summary>
        /// Get list of Associated Camp.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMCampUserListViewModel</returns>
        DBTMCampUserListViewModel GetDBTMCampUserList(int dBTMCampMasterId, string userType, DataTableViewModel dataTableModel);

        /// <summary>
        /// Update Associate UnAssociate Campwise User.
        /// </summary>
        /// <param name="DBTMCampUserViewModel">DBTMCampUserViewModel.</param>
        /// <returns>Returns updated DBTMCampUserViewModel</returns>
        DBTMCampUserViewModel AssociateUnAssociateCampwiseUser(DBTMCampUserViewModel dBTMCampUserViewModel);
        DBTMCampUserListViewModel GetCampUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, long DBTMCampMasterId);
    }
}
