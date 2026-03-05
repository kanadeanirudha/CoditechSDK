using Coditech.Admin.ViewModel;
namespace Coditech.Admin.Agents
{
    public interface IDBTMCentreWiseSettingAgent
    {

        /// <summary>
        /// Get DBTMPrivacySetting by DBTMPrivacySettingId.
        /// </summary>
        /// <param name="dBTMCentreWiseSettingId">dBTMCentreWiseSettingId</param>
        /// <returns>Returns DBTMCentreWiseSettingViewModel.</returns>
        DBTMCentreWiseSettingViewModel GetDBTMCentreWiseSetting(int organisationCentreId);

        /// <summary>
        /// Update DBTM Centre Wise Setting.
        /// </summary>
        /// <param name="dBTMCentreWiseSettingViewModel">dBTMCentreWiseSettingViewModel.</param>
        /// <returns>Returns updated DBTMCentreWiseSettingViewModel</returns>
        DBTMCentreWiseSettingViewModel UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingViewModel dBTMCentreWiseSettingViewModel);
        DBTMCentreWiseTestViewModel AssociateUnAssociateCentreTest(DBTMCentreWiseTestViewModel dBTMCentreWiseTestViewModel);
    }
}

