using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMCampClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTM Camp.
        /// </summary>
        /// <returns>DBTMCampListResponse</returns>
        DBTMCampListResponse List(string selectedCentreCode, long userMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMCampMaster.
        /// </summary>
        /// <param name="DBTMCampMasterModel">DBTMCampMasterModel.</param>
        /// <returns>Returns DBTMCampResponse.</returns>
        DBTMCampResponse CreateDBTMCamp(DBTMCampMasterModel body);

        /// <summary>
        /// Get DBTMCampMaster by DBTMCampMasterId.
        /// </summary>
        /// <param name="CampCode">CampCode</param>
        /// <returns>Returns DBTMCampMasterResponse.</returns>
        DBTMCampResponse GetDBTMCamp(long dBTMCampMasterId);

        /// <summary>
        /// Update DBTMCampMaster.
        /// </summary>
        /// <param name="DBTMCampMasterModel">DBTMCampMasterModel.</param>
        /// <returns>Returns updated DBTMCampMasterResponse</returns>
        DBTMCampResponse UpdateDBTMCamp(DBTMCampMasterModel body);

        /// <summary>
        /// Delete DBTMCampMaster.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMCamp(ParameterModel body);

        /// <summary>
        /// Get list of DBTMCampUser.
        /// </summary>
        /// <returns>DBTMCampUserListResponse</returns>
        DBTMCampUserListResponse GetDBTMCampUserList(long dBTMCampMasterId, string userType, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Update Associate UnAssociate Campwise User.
        /// </summary>
        /// <param name="DBTMCampUserModel">DBTMCampUserModel.</param>
        /// <returns>Returns updated DBTMCampUserResponse</returns>
        DBTMCampUserResponse AssociateUnAssociateCampwiseUser(DBTMCampUserModel body);
        DBTMCampUserListResponse GetCampUserListByCentreCodeAndGeneralTrainerMasterId(string selectedCentreCode, long generalTrainerMasterId, long dBTMCampMasterId);
    }
}
