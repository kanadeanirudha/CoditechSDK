using Coditech.Admin.ViewModel;
using Coditech.Common.API.Model.Response;

namespace Coditech.Admin.Agents
{
    public interface IDBTMPrivacySettingAgent
    {
        /// <summary>
        /// Get list of DBTM PrivacySetting .
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMPrivacySettingListViewModel</returns>
        DBTMPrivacySettingListViewModel GetDBTMPrivacySettingList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTMPrivacySetting.
        /// </summary>
        /// <param name="dBTMPrivacySettingViewModel"> DBTM DBTMPrivacySetting  View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMPrivacySettingViewModel CreateDBTMPrivacySetting(DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel);

        /// <summary>
        /// Get DBTMPrivacySetting by DBTMPrivacySettingId.
        /// </summary>
        /// <param name="dBTMPrivacySettingId">dBTMPrivacySettingId</param>
        /// <returns>Returns DBTMPrivacySettingViewModel.</returns>
        DBTMPrivacySettingViewModel GetDBTMPrivacySetting(int dBTMPrivacySettingId);

        /// <summary>
        /// Update DBTMPrivacySettingId.
        /// </summary>
        /// <param name="dBTMPrivacySettingViewModel">dBTMPrivacySettingViewModel.</param>
        /// <returns>Returns updated DBTMPrivacySettingViewModel</returns>
        DBTMPrivacySettingViewModel UpdateDBTMPrivacySetting(DBTMPrivacySettingViewModel dBTMPrivacySettingViewModel);

        /// <summary>
        /// Delete DBTMPrivacySetting.
        /// </summary>
        /// <param name="dBTMPrivacySettingId">dBTMPrivacySettingId.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMPrivacySetting(string dBTMPrivacySettingId, out string errorMessage);
        //DBTMPrivacySettingListResponse GetDBTMPrivacySettingList();
    }
}
