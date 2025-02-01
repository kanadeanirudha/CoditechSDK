using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;
namespace Coditech.API.Client
{
    public interface IDBTMPrivacySettingClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMPrivacySetting.
        /// </summary>
        /// <returns>DBTMPrivacySettingListResponse</returns>
        DBTMPrivacySettingListResponse List(string SelectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMPrivacySetting.
        /// </summary>
        /// <param name="DBTMPrivacySettingModel">DBTMPrivacySettingModel.</param>
        /// <returns>Returns DBTMPrivacySettingResponse.</returns>
        DBTMPrivacySettingResponse CreateDBTMPrivacySetting(DBTMPrivacySettingModel body);

        /// <summary>
        /// Get DBTMPrivacySetting by DBTMPrivacySettingId.
        /// </summary>
        /// <param name="dBTMPrivacySettingId">dBTMPrivacySettingId</param>
        /// <returns>Returns DBTMPrivacySettingResponse.</returns>
        DBTMPrivacySettingResponse GetDBTMPrivacySetting(int dBTMPrivacySettingId);

        /// <summary>
        /// Update DBTMPrivacySetting.
        /// </summary>
        /// <param name="DBTMPrivacySettingModel">DBTMPrivacySettingModel.</param>
        /// <returns>Returns updated DBTMPrivacySettingResponse</returns>
        DBTMPrivacySettingResponse UpdateDBTMPrivacySetting(DBTMPrivacySettingModel model);

        /// <summary>
        /// Delete DBTMPrivacySetting.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMPrivacySetting(ParameterModel body);
    }
}
