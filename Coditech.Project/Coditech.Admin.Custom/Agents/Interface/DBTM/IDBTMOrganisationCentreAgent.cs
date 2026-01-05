using Coditech.Admin.ViewModel;
namespace Coditech.Admin.Agents
{
    public interface IDBTMOrganisationCentreAgent
    {    
        /// <summary>
        /// Get ActivityListViewSequence by dBTMOrganisationCentreMasterId.
        /// </summary>
        /// <param name="dBTMOrganisationCentreMasterId">dBTMOrganisationCentreMasterId</param>
        /// <returns>Returns DBTMDeviceViewModel.</returns>
        DBTMActivityListViewSequenceListViewModel GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, DataTableViewModel dataTableModel);

        /// <summary>
        /// Get DBTMCentrewiseTestParameterListView by dBTMOrganisationCentreMasterId.
        /// </summary>
        /// <param name="dBTMOrganisationCentreMasterId">dBTMOrganisationCentreMasterId</param>
        /// <returns>Returns DBTMDeviceViewModel.</returns>
        DBTMCentrewiseTestParameterListViewViewModel GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId);

        /// <summary>
        /// Update DBTMCentrewiseTestParameterListView.
        /// </summary>
        /// <param name="dBTMCentrewiseTestParameterListViewViewModel">dBTMCentrewiseTestParameterListViewViewModel.</param>
        /// <returns>Returns updated dBTMCentrewiseTestParameterListViewViewModel</returns>
        DBTMCentrewiseTestParameterListViewViewModel UpdateDBTMCentrewiseTestParameterListView(DBTMCentrewiseTestParameterListViewViewModel dBTMCentrewiseTestParameterListViewViewModel);
    }
}
