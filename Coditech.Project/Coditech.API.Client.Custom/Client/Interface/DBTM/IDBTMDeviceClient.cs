using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMDeviceClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMDevice.
        /// </summary>
        /// <returns>DBTMDeviceListResponse</returns>
        DBTMDeviceListResponse List(IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMDevice.
        /// </summary>
        /// <param name="DBTMDeviceModel">DBTMDeviceModel.</param>
        /// <returns>Returns DBTMDeviceResponse.</returns>
        DBTMDeviceResponse CreateDBTMDevice(DBTMDeviceModel body);

        /// <summary>
        /// Get DBTMDevice by dBTMDeviceId.
        /// </summary>
        /// <param name="dBTMDeviceId">dBTMDeviceId</param>
        /// <returns>Returns DBTMDeviceResponse.</returns>
        DBTMDeviceResponse GetDBTMDevice(long dBTMDeviceId);

        /// <summary>
        /// Update DBTMDevice.
        /// </summary>
        /// <param name="DBTMDeviceModel">DBTMDeviceModel.</param>
        /// <returns>Returns updated DBTMDeviceResponse</returns>
        DBTMDeviceResponse UpdateDBTMDevice(DBTMDeviceModel model);

        /// <summary>
        /// Delete DBTMDevice.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMDevice(ParameterModel body);
    }
}
