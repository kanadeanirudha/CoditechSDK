using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMTraineeDetailsClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMTraineeDetails.
        /// </summary>
        /// <returns>DBTMTraineeDetailsListResponse</returns>
        DBTMTraineeDetailsListResponse List(string selectedCentreCode, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Get DBTM Trainee Other Details by dBTMTraineeDetailId.
        /// </summary>
        /// <param name="dBTMTraineeDetailId">dBTMTraineeDetailId</param>
        /// <returns>Returns DBTMTraineeDetailsResponse.</returns>
        DBTMTraineeDetailsResponse GetDBTMTraineeOtherDetails(long dBTMTraineeDetailId);

        /// <summary>
        /// Update DBTM Trainee Other Details.
        /// </summary>
        /// <param name="DBTMTraineeDetailsModel">DBTMTraineeDetailsModel.</param>
        /// <returns>Returns updated DBTMTraineeDetailsResponse</returns>
        DBTMTraineeDetailsResponse UpdateDBTMTraineeOtherDetails(DBTMTraineeDetailsModel body);

        /// <summary>
        /// Delete DBTMTraineeDetails.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMTraineeDetails(ParameterModel body);

        DBTMActivitiesListResponse GetTraineeActivitiesList(string personCode,int numberOfDaysRecord, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);
        DBTMActivitiesDetailsListResponse GetTraineeActivitiesDetailsList(long dBTMDeviceDataId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);
    }
}
