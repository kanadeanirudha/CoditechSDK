using Coditech.Admin.ViewModel;
namespace Coditech.Admin.Agents
{
    public interface IDBTMDashboardAgent
    {
        /// <summary>
        /// Get GetDBTMDashboardDetails.
        /// </summary>
        /// <returns>Returns DBTMDashboardViewModel.</returns>
        DBTMDashboardViewModel GetDBTMDashboardDetails(short numberOfDaysRecord);
        DBTMDashboardViewModel GetTrainerDashBoard(short numberOfDaysRecord, long generalTrainerMasterId, int adminRoleMasterId, long userMasterId);
        UserProfileViewModel GetUserProfile( long userMasterId);
        GeneralBatchListViewModel GetBatchList(DataTableViewModel dataTableModel);
    }
}
