using Coditech.Common.API.Model;
using Coditech.Common.API.Model.Response;
using Coditech.Common.API.Model.Responses;
using Coditech.Common.Helper.Utilities;

namespace Coditech.API.Client
{
    public interface IDBTMOrganisationCentreClient : IBaseClient
    {
        /// <summary>
        /// Get ActivityListViewSequence by dBTMOrganisationCentreMasterId.
        /// </summary>
        /// <param name="dBTMOrganisationCentreMasterId">dBTMOrganisationCentreMasterId</param>
        /// <returns>Returns DBTMOrganisationCentreResponse.</returns>   
        DBTMActivityListViewSequenceListResponse GetActivityListViewSequenceList(int dBTMOrganisationCentreMasterId, IEnumerable<string> expand, IEnumerable<FilterTuple> filter, IDictionary<string, string> sort, int? pageIndex, int? pageSize);

        /// <summary>
        /// Get DBTMCentrewiseTestParameterListView by DBTMOrganisationCentreParameterListViewSequenceId.
        /// </summary>
        /// <param name="DBTMOrganisationCentreParameterListViewSequenceId">DBTMOrganisationCentreParameterListViewSequenceId</param>
        /// <returns>Returns DBTMCentrewiseTestParameterListViewResponse.</returns>
        DBTMCentrewiseTestParameterListViewResponse GetDBTMCentrewiseTestParameterListView(int dBTMOrganisationCentreParameterListViewSequenceId);

        /// <summary>
        /// Update DBTMCentrewiseTestParameterListView.
        /// </summary>
        /// <param name="DBTMCentrewiseTestParameterListViewModel">DBTMCentrewiseTestParameterListViewModel.</param>
        /// <returns>Returns updated DBTMCentrewiseTestParameterListViewResponse</returns>
        DBTMCentrewiseTestParameterListViewResponse UpdateDBTMCentrewiseTestParameterListView(DBTMCentrewiseTestParameterListViewModel model);
    }
}
