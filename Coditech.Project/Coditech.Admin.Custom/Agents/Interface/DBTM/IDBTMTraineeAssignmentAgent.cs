using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMTraineeAssignmentAgent
    {
        /// <summary>
        /// Get list of DBTMTraineeAssignment.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMTraineeAssignmentListViewModel</returns>
        DBTMTraineeAssignmentListViewModel GetDBTMTraineeAssignmentList(DataTableViewModel dataTableModel);

        /// <summary>
        /// Create DBTMTraineeAssignment.
        /// </summary>
        /// <param name="dBTMTraineeAssignmentViewModel">DBTM Trainee Assignment View Model.</param>
        /// <returns>Returns created model.</returns>
        DBTMTraineeAssignmentViewModel CreateDBTMTraineeAssignment(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel);

        /// <summary>
        /// Get DBTMTraineeAssignment by dBTMTraineeAssignmentId.
        /// </summary>
        /// <param name="dBTMTraineeAssignmentId">dBTMTraineeAssignmentId</param>
        /// <returns>Returns DBTMTraineeAssignmentViewModel.</returns>
        DBTMTraineeAssignmentViewModel GetDBTMTraineeAssignment(long dBTMTraineeAssignmentId);

        /// <summary>
        /// Update DBTMTraineeAssignment.
        /// </summary>
        /// <param name="dBTMTraineeAssignmentViewModel">dBTMTraineeAssignmentViewModel.</param>
        /// <returns>Returns updated DBTMTraineeAssignmentViewModel</returns>
        DBTMTraineeAssignmentViewModel UpdateDBTMTraineeAssignment(DBTMTraineeAssignmentViewModel dBTMTraineeAssignmentViewModel);

        /// <summary>
        /// Delete DBTMTraineeAssignmentViewModel.
        /// </summary>
        /// <param name="dBTMTraineeAssignmentIds">dBTMTraineeAssignmentIds.</param>
        /// <returns>Returns true if deleted successfully else return false.</returns>
        bool DeleteDBTMTraineeAssignment(string dBTMTraineeAssignmentIds, out string errorMessage);
        bool SendAssignmentReminder(string dBTMTraineeAssignmentId, out string errorMessage);
    }
}
