using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMTraineeAssignmentClient : IBaseClient
    {
        /// <summary>
        /// Get list of DBTMTraineeAssignment.
        /// </summary>
        /// <returns>DBTMTraineeAssignmentListResponse</returns>
        DBTMTraineeAssignmentListResponse List(long generalTrainerMasterId,IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Create DBTMTraineeAssignment.
        /// </summary>
        /// <param name="DBTMTraineeAssignmentModel">DBTMTraineeAssignmentModel.</param>
        /// <returns>Returns DBTMTraineeAssignmentResponse.</returns>
        DBTMTraineeAssignmentResponse CreateDBTMTraineeAssignment(DBTMTraineeAssignmentModel body);

        /// <summary>
        /// Get DBTMTraineeAssignment by dBTMTraineeAssignmentId.
        /// </summary>
        /// <param name="dBTMTraineeAssignmentId">dBTMTraineeAssignmentId</param>
        /// <returns>Returns DBTMTraineeAssignmentResponse.</returns>
        DBTMTraineeAssignmentResponse GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId);

        /// <summary>
        /// Update DBTMTraineeAssignment.
        /// </summary>
        /// <param name="DBTMTraineeAssignmentModel">DBTMTraineeAssignmentModel.</param>
        /// <returns>Returns updated DBTMTraineeAssignmentResponse</returns>
        DBTMTraineeAssignmentResponse UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentModel model);

        /// <summary>
        /// Delete DBTMTraineeAssignment.
        /// </summary>
        /// <param name="ParameterModel">ParameterModel.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        TrueFalseResponse DeleteDBTMTraineeAssignment(ParameterModel body);

        /// <summary>
        /// Get list of DBTM Trainer.
        /// </summary>
        /// <returns>DBTMTraineeAssignmentListResponse</returns>
        GeneralTrainerListResponse GetTrainerByCentreCode(string centreCode);
        DBTMTraineeDetailsListResponse GetTraineeDetailByCentreCodeAndgeneralTrainerId(string centreCode, long generalTrainerId);
        TrueFalseResponse SendAssignmentReminder(string dBTMTraineeAssignmentId);
    }
}
