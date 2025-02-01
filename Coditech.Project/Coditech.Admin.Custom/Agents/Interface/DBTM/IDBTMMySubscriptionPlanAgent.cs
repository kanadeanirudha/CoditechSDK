using Coditech.Admin.ViewModel;

namespace Coditech.Admin.Agents
{
    public interface IDBTMMySubscriptionPlanAgent
    {
        /// <summary>
        /// Get list of DBTM My Subscription Plan.
        /// </summary>
        /// <param name="dataTableModel">DataTable ViewModel.</param>
        /// <returns>DBTMMySubscriptionPlanListViewModel</returns>
        DBTMMySubscriptionPlanListViewModel GetDBTMMySubscriptionPlanList(DataTableViewModel dataTableModel);
    }
}
