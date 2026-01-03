using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Responses;
namespace Coditech.API.Client
{
    public interface IDBTMCentreWiseSettingClient : IBaseClient
    {
        /// <summary>
        /// Get DBTMCentreWiseSetting by dBTMCentreWiseSettingId.
        /// </summary>
        /// <param name="dBTMPrivacySettingId">dBTMPrivacySettingId</param>
        /// <returns>Returns DBTMPrivacySettingResponse.</returns>
        DBTMCentreWiseSettingResponse GetDBTMCentreWiseSetting(int organisationCentreId);

        /// <summary>
        /// Update DBTMCentreWiseSetting.
        /// </summary>
        /// <param name="DBTMCentreWiseSettingModel">DBTMCentreWiseSettingModel.</param>
        /// <returns>Returns updated DBTMCentreWiseSettingResponse</returns>
        DBTMCentreWiseSettingResponse UpdateDBTMCentreWiseSetting(DBTMCentreWiseSettingModel model);

      
    }
}
