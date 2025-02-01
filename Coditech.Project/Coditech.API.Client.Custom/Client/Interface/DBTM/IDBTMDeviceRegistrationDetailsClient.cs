using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMDeviceRegistrationDetailsClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMDeviceRegistrationDetails.
        /// </summary>
        /// <returns>DBTMDeviceRegistrationDetailsListResponse</returns>
        DBTMDeviceRegistrationDetailsListResponse List(long UserMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMDeviceRegistrationDetails.
        /// </summary>
        /// <param name="DBTMDeviceRegistrationDetailsModel">DBTMDeviceRegistrationDetailsModel.</param>
        /// <returns>Returns DBTMDeviceRegistrationDetailsResponse.</returns>
        DBTMDeviceRegistrationDetailsResponse CreateRegistrationDetails(DBTMDeviceRegistrationDetailsModel body);

        /// <summary>
        /// Get DBTMDeviceRegistrationDetails by dBTMDeviceRegistrationDetailId.
        /// </summary>
        /// <param name="dBTMDeviceRegistrationDetailId">dBTMDeviceRegistrationDetailId</param>
        /// <returns>Returns DBTMDeviceRegistrationDetailsResponse.</returns>
        DBTMDeviceRegistrationDetailsResponse GetRegistrationDetails(long dBTMDeviceRegistrationDetailId);

        /// <summary>
        /// Update DBTMDeviceRegistrationDetails.
        /// </summary>
        /// <param name="DBTMDeviceRegistrationDetailsModel">DBTMDeviceRegistrationDetailsModel.</param>
        /// <returns>Returns updated DBTMDeviceRegistrationDetailsResponse</returns>
        DBTMDeviceRegistrationDetailsResponse UpdateRegistrationDetails(DBTMDeviceRegistrationDetailsModel model);

        /// <summary>
        /// Delete DBTMDeviceRegistrationDetails.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteRegistrationDetails(ParameterModel body);
    }
}
