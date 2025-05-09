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
        DBTMTraineeAssignmentViewModel SendAssignmentReminder(long dBTMTraineeAssignmentId);

        /// <summary>
        /// Get list of Associated Assignment.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMTraineeAssignmentToUserListViewModel</returns>
        DBTMTraineeAssignmentToUserListViewModel GetDBTMTraineeAssignmentToUserList(long dBTMTraineeAssignmentId, DataTableViewModel dataTableModel);

        /// <summary>
        /// Update Associate UnAssociate Assignmentwise User.
        /// </summary>
        /// <param name="DBTMTraineeAssignmentToUserViewModel">DBTMTraineeAssignmentToUserViewModel.</param>
        /// <returns>Returns updated DBTMTraineeAssignmentToUserViewModel</returns>
        DBTMTraineeAssignmentToUserViewModel AssociateUnAssociateAssignmentwiseUser(DBTMTraineeAssignmentToUserViewModel DBTMTraineeAssignmentToUserViewModel);
    }
}
